(function () {

    'use strict';

    var app = angular.module('app', ['ngRoute']);

    app.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

        $locationProvider.html5Mode(true);

        $routeProvider
            .when('/blogs', {
                controller: 'BlogsController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/blogs.html'
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
            .otherwise('/blogs');
    }]);

})();