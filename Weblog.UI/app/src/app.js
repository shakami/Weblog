(function () {

    'use strict';

    var app = angular.module('app', ['ngRoute']);

    app.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

        $locationProvider.html5Mode(true);

        $routeProvider
            .when('/browse', {
                controller: 'BrowseController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/browse.html'
            })
            .when('/users/:userId/blogs/:blogId', {
                controller: 'BlogController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/blog.html'
            })
            .when('/users/:userId/blogs/:blogId/posts/:postId', {
                controller: 'PostController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/post.html'
            })
            .when('/latest', {
                controller: 'LatestController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/latest.html'
            })
            .when('/register', {
                controller: 'RegisterController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/register.html'
            })
            .otherwise('/latest');
    }]);

})();