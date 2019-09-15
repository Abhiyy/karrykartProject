var app = angular.module("karrykartApp", ['ngSanitize', 'ui.bootstrap']);

app.service('GlobalService', ['$http', function ($http) {

    //Dev    
    //this.apiURL = 'http://localhost:13518/api/';
    //this.websiteURL = 'http://localhost:15557/';

    //Test
    this.apiURL = 'http://testapi.karrykart.com/api/';
    this.websiteURL = 'http://test.karrykart.com/';

    //Production
    //this.apiURL = 'http://api.karrykart.com/api/';
    //this.websiteURL = 'http://www.karrykart.com/';

    this.isFilterOn = false;
}]);