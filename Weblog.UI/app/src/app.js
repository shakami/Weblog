(function () {

    'use strict';

    var app = angular.module('app', ['ngRoute']);

    app.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

        $locationProvider.html5Mode(true);

        $routeProvider
            .when('/blogs', {
                controller: 'BlogsController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/blogs.html',
            })
            .otherwise('/blogs');
    }]);

})();