app.controller("ProductSearchController", ['$scope', '$location', '$http', '$window', 'GlobalService', function ($scope, $location, $http, $window, GlobalService) {

    $scope.AvailableProducts = [];
    $scope.selectedProduct = undefined;
 
    loadAllProducts();
 
    

    function loadAllProducts() {

        $http({
            method: 'GET',
            url: GlobalService.apiURL + "product",
        }).then(function successCallback(response) {
            angular.forEach(response.data, function (result, key) {
                $scope.AvailableProducts.push(result.Name);
            });
        }, function errorCallback(response) {
            console.log('Oops! Something went wrong while fetching the data. Status Code: ' + response.status + ' Status statusText: ' + response.statusText);
        });

    }

    $scope.searchProduct = function () {

        $window.location.href = GlobalService.websiteURL + 'Search/Results#?search=' + $scope.selectedProduct;// + $location.search('search',);
    };
}
]);