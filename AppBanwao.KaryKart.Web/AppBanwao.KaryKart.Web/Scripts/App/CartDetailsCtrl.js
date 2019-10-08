app.controller("CartDetailsController", ['$scope', '$http', '$window', '$location', 'GlobalService', '$localStorage', '$rootScope', 'cartService', function ($scope, $http, $window, $location, GlobalService, $localStorage, $rootScope, cartService) {
    let apiURL = GlobalService.apiURL + "cart?CartID=";
    $scope.cart;

    loadCart();


    function loadCart()
    {
        let cart = localStorage.getItem('cart')!=null ? JSON.parse(localStorage.getItem('cart')) : null;
        if(cart!=null){
            $http({
                method: "GET",
                url: apiURL+cart.CartID
            }).success(function (data, status, header, config) {
                $scope.cart = data;
                console.log($scope.cart);

            
            });
        }
    }
}]);
