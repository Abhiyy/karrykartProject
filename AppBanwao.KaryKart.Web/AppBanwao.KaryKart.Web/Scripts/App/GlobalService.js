var app = angular.module("karrykartApp", ['ngSanitize', 'ui.bootstrap']);

app.service('GlobalService', ['$http', function ($http) {
    this.apiURL = 'http://localhost:13518/api/';
    this.websiteURL = 'http://localhost:15557/';
    this.isFilterOn = false;
}]);