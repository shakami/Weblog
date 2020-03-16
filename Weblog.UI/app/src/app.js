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
            .when('/users/:userId', {
                controller: 'UserProfileController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/user-profile.html'
            })
            .when('/users/:userId/blogs', {
                controller: 'BrowseController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/browse.html'
            })
            .when('/users/:userId/blogs/:blogId', {
                controller: 'BlogController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/blog.html'
            })
            .when('/users/:userId/blogs/:blogId/edit', {
                controller: 'BlogEditController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/blog-form.html'
            })
            .when('/new-blog', {
                controller: 'BlogCreateController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/blog-form.html'
            })
            .when('/users/:userId/blogs/:blogId/posts/new', {
                controller: 'NewPostController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/new-post.html'
            })
            .when('/users/:userId/blogs/:blogId/posts/:postId', {
                controller: 'PostController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/post.html'
            })
            .when('/register', {
                controller: 'RegisterController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/register.html'
            })
            .when('/unauthorized', {
                templateUrl: '/app/src/templates/unauthorized.html'
            })
            .otherwise('/browse');
    }]);

})();