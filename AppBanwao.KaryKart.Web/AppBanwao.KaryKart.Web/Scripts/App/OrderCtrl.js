app.controller("OrderController", ['$scope', '$http', '$window', '$location', 'GlobalService', function ($scope, $http, $window, $location, GlobalService) {
    $scope.basicDetailsEditing = false;
    var apiURL = GlobalService.apiURL + "OrderInformation";
    var orderID = $location.absUrl().substring(($location.absUrl().lastIndexOf("id=") + 3));
    $scope.selected = {};
    $scope.isAddPSS = false;
    $scope.orderstatuses;
    $scope.categories;
    $scope.subcategories;
    $scope.brands;
    $scope.isAddFeature = false;
    $scope.isAddImage = false;
    $scope.sizetypes;
    $scope.units;
    $scope.sizes;

    $http.get("/order/GetOrderStatus").success(function (data) {
        $scope.orderstatuses = data;
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

    LoadOrder();

    function LoadOrder() {
        $http({
            method: "GET",
            url: apiURL,
            params: {'OrderID': orderID}
        }).success(function (data, status, header, config) {
            console.log(data);
            $scope.order = data;
        });
}

    $scope.updateOrder = function () {
        $http.post("/order/UpdateOrderStatus", { OrderID: $scope.order.OrderID, Status: $scope.order.OrderStatusID, UserEmail: $scope.order.User.Email }).success(function (responseData) {
            if (responseData.messagetype = "success") {
                alert(responseData.message);
                LoadOrder();
            } else {
                alert(responseData.message);
            }
        }).error(function (responseData) {
            console.log("Error !" + responseData);
        });
    };

    $scope.adjustOrderAmount = function () {
        $http.post("/order/AdjustOrderAmount", { PaymentID: $scope.order.PaymentID, Amount: $scope.order.TotalAmount }).success(function (responseData) {
            if (responseData.messagetype = "success") {
                alert(responseData.message);
                LoadOrder();
            } else {
                alert(responseData.message);
            }
        }).error(function (responseData) {
            console.log("Error !" + responseData);
        });
    };

    $scope.UpdatePaymentStatus = function () {
        $http.post("/order/UpdatePaymentStatus", { PaymentID: $scope.order.PaymentID, update: $scope.order.isPaymentSuccessful }).success(function (responseData) {
            if (responseData.messagetype = "success") {
                alert(responseData.message);
                LoadOrder();
            } else {
                alert(responseData.message);
            }
        }).error(function (responseData) {
            console.log("Error !" + responseData);
        });
    };

}]);