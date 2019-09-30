app.controller("SearchResultsController", ['$scope', '$http', '$location', '$window', 'GlobalService', function ($scope, $http, $location, $window, GlobalService) {

    $scope.searchedProductList;
    $scope.searchedProduct = $location.search()['search'];
    GetSearchedProduct();

    function GetSearchedProduct() {

        $http({
            method: 'GET',
            url: GlobalService.apiURL + "Search?ProductName=" + $scope.searchedProduct

        }).success(function (data, status, header, config) {
            $scope.searchedProductList = data;
            $scope.isFilterOn = true;
        });
    }

    $scope.viewProduct = function (productID) {
     $window.location.href = GlobalService.websiteURL + 'ProductDetails/Index/' + productID;
    };
}
]);