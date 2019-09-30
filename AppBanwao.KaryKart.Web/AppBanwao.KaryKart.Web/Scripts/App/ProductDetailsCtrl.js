app.controller("ProductDetailsController", ['$scope', '$http', '$window', '$location', 'GlobalService', '$localStorage', function ($scope, $http, $window, $location, GlobalService, $localStorage) {
    $scope.basicDetailsEditing = false;
    var apiURL = GlobalService.apiURL + "product?id=" + $location.absUrl().substring(($location.absUrl().lastIndexOf("/") + 1));//$http.get(apiURL + "product?id=" + $location.absUrl().substring(($location.absUrl().lastIndexOf("/") + 1));
    $scope.selected = {};
    $scope.isAddPSS = false;
    $scope.categories;
    $scope.subcategories;
    $scope.brands;
    $scope.isAddFeature = false;
    $scope.isAddImage = false;
    $scope.sizetypes;
    $scope.units;
    $scope.sizes;
    $scope.fullImageSrc;
    $scope.inStockClass=false;
    $scope.quantity = 1;
    $scope.cartPrice = 1;
    $scope.quantities =[];
    $scope.quantitylimit = 30;
    $scope.shippingCost = 0;
    $scope.cart;
    $http.get("/Product/GetCategories").success(function (data) {
        $scope.categories = data;
    }).error(function (status) {
        //  alert(status);
    });

    $http.get("/Product/GetBrands").success(function (data) {
        $scope.brands = data;
    }).error(function (status) {
        //  alert(status);
    });

    $http.get("/Product/GetSubCategories").success(function (data) {
        $scope.subcategories = data;
    }).error(function (status) {
        //  alert(status);
    });

    $http.get("/Product/GetSizeTypes").success(function (data) {
        $scope.sizetypes = data;
    }).error(function (status) {
        //  alert(status);
    });

    $http.get("/Product/GetUnits").success(function (data) {
        $scope.units = data;
    }).error(function (status) {
        //  alert(status);
    });


    $http.get("/Product/GetSizes").success(function (data) {
        $scope.sizes = data;
    }).error(function (status) {
        //  alert(status);
    });

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
        console.log(sessionStorage.getItem('UserID'));
        if (localStorage.getItem('cart') != null) {
            // create cart
            //showNotification("Item added to cart.", 4000);
            //check for user session
            
            if (sessionStorage.getItem('UserID') != null) {
                $scope.cart = {
                    "ProductID": $scope.product.ProductID,
                    "CreateCart": false,
                    "Quantity": $scope.quantity,
                    "ProductCount": $scope.quantity,
                    "CartID": localStorage.getItem('cart'),
                    "User": sessionStorage.getItem('UserID')
                };
            } else {
                $scope.cart = {
                    "ProductID": $scope.product.ProductID,
                    "CreateCart": false,
                    "Quantity": $scope.quantity,
                    "ProductCount": $scope.quantity,
                    "CartID": localStorage.getItem('cart')
                 };
            }
        } else {
            //update cart
            alert('Helllo');
        }
    };
    
    $scope.createCart = function (data) {
        $http.post(GlobalService.apiURL+"/cart", { cart: JSON.stringify(data) }).success(function (responseData) {
            if (responseData.messagetype = "success") {
                alert(responseData.message);
                LoadProduct();
                showNotification("Item added to cart.", 4000);

            } else {
                alert(responseData.message);
            }
        }).error(function (responseData) {
            console.log("Error !" + responseData);
        });
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