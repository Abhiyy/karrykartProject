app.controller("ProductDetailsController", ['$scope', '$http', '$window', '$location', 'GlobalService', '$localStorage', '$rootScope', 'cartService', function ($scope, $http, $window, $location, GlobalService, $localStorage, $rootScope,cartService) {
    $scope.basicDetailsEditing = false;
    var apiURL = GlobalService.apiURL + "product?id=" + $location.absUrl().substring(($location.absUrl().lastIndexOf("/") + 1));//$http.get(apiURL + "product?id=" + $location.absUrl().substring(($location.absUrl().lastIndexOf("/") + 1));
    $scope.selected = {};
    $scope.isAddPSS = false;
    $scope.fullImageSrc;
    $scope.inStockClass=false;
    $scope.quantity = 1;
    $scope.cartPrice = 1;
    $scope.quantities =[];
    $scope.quantitylimit = 30;
    $scope.shippingCost = 0;
    $scope.cart;
    
    LoadProduct();
    BuildQuantity();

    function BuildQuantity()
    {
        for (var i = 1; i <= $scope.quantitylimit; i++)
        {
            $scope.quantities.push(i);
        }
    }
    function LoadProduct() {
         
        $http({
            method: "GET",
            url: apiURL
        }).success(function (data, status, header, config) {
            $scope.product = data;
            $scope.productPriceStockMappings = [];
            // var json = JSON.parse(JSON.stringify(data));
            for (var i = 0; i < data.ProductSizeMappings.length; i++) {
                $scope.productPriceStockMappings.push({
                    "ProductSizeMappingID": $scope.product.ProductSizeMappings[i].ProductSizeMappingID,
                    "SizeID": $scope.product.ProductSizeMappings[i].SizeID,
                    "SizeName": $scope.product.ProductSizeMappings[i].SizeName,
                    "UnitID": $scope.product.ProductSizeMappings[i].UnitID,
                    "UnitName": $scope.product.ProductSizeMappings[i].UnitName,
                    "SizeTypeID": $scope.product.ProductSizeMappings[i].SizeTypeID,
                    "SizeTypeName": $scope.product.ProductSizeMappings[i].SizeTypeName,
                    "Stock": $scope.product.ProductSizeMappings[i].Stock,
                    "ShippingCostID": $scope.product.ShippingDetails[i].ShippingCostID,
                    "Cost": $scope.product.ShippingDetails[i].Cost,
                    "PriceID": $scope.product.Prices[i].PriceID,
                    "Price": $scope.product.Prices[i].Price,
                    "CurrencyID": $scope.product.Prices[i].CurrencyID,
                });
            }
            $scope.fullImageSrc = data.Images[0].ImageLink;
            $scope.cartPrice = $scope.product.Prices[0].Price * $scope.quantity;
            $scope.shippingCost = $scope.product.ShippingDetails[0].Cost * $scope.quantity;
            $scope.inStockClass = data.ProductSizeMappings[0].Stock > 0 ? true : false;
            
        });

      
    }


    $scope.changeQuantity = function () {
        $scope.cartPrice = $scope.product.Prices[0].Price * $scope.quantity;
        $scope.shippingCost = $scope.product.ShippingDetails[0].Cost * $scope.quantity;
    }
    $scope.loadImage = function (pImage) {
        $scope.fullImageSrc = pImage.ImageLink;
    }

    $scope.addToCart = function () {
        let cart = localStorage.getItem('cart')!=null?JSON.parse(localStorage.getItem('cart')):null;
        let userID = localStorage.getItem('UserID'); 
        if (cart != null) {
            // update cart
            console.log(cart);
            if (userID != null && userID != "") {
                $scope.cart = {
                    "ProductID": $scope.product.ProductID,
                    "CreateCart": false,
                    "Quantity": $scope.quantity,
                    "ProductCount": $scope.quantity,
                    "CartID": cart.CartID,
                    "User": userID
                };
            } else {
                $scope.cart = {
                    "ProductID": $scope.product.ProductID,
                    "CreateCart": false,
                    "Quantity": $scope.quantity,
                    "ProductCount": $scope.quantity,
                    "CartID": cart.CartID
                 };
            }
            $http.put(GlobalService.apiURL + "cart",  $scope.cart ).success(function (responseData) {
                
                localStorage.setItem("cart", JSON.stringify(responseData));
                LoadProduct();
                // cartService.updateCart(responseData, responseData.ProductCount);
                $scope.raiseCartUpdate('cartUpdate', responseData.ProductCount);
                showNotification("Item added to cart.", 4000);
                //  $scope.cartCount = responseData.ProductCount;

            }).error(function (responseData) {
                console.log("Error !" + responseData);
            });

        } else {
            //create cart
            if (localStorage.getItem('UserID') != null && localStorage.getItem('UserID') != "") {
                $scope.cart = {
                    "ProductID": $scope.product.ProductID,
                    "CreateCart": true,
                    "Quantity": $scope.quantity,
                    "ProductCount": $scope.quantity,
                    "User": sessionStorage.getItem('UserID')
                };
            } else {
                $scope.cart = {
                    "ProductID": $scope.product.ProductID,
                    "CreateCart": true,
                    "Quantity": $scope.quantity,
                    "ProductCount": $scope.quantity
                };
            }

            $http.post(GlobalService.apiURL + "cart",  $scope.cart ).success(function (responseData) {
                
                localStorage.setItem("cart", JSON.stringify(responseData));
                LoadProduct();
               // cartService.updateCart(responseData, responseData.ProductCount);
                $scope.raiseCartUpdate('cartUpdate', responseData.ProductCount);
                showNotification("Item added to cart.", 4000);
              //  $scope.cartCount = responseData.ProductCount;

            }).error(function (responseData) {
                console.log("Error !" + responseData);
            });
        }
    };
    
    $scope.raiseCartUpdate = function (eventName, data) {
        $rootScope.$emit(eventName, data);
     
    };

    $scope.deleteProductFeature = function (id) {
        $http.post("/Product/RemoveProductFeature", { ProductID: $scope.product.ProductID, FeatureID: id }).success(function (responseData) {
            if (responseData.messagetype = "success") {
                alert(responseData.message);
                LoadProduct();
                
            } else {
                alert(responseData.message);
            }
        }).error(function (responseData) {
            console.log("Error !" + responseData);
        });
    };

    
}]);