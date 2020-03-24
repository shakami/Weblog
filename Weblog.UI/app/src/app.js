(function () {

    'use strict';

    var app = angular.module('app', ['ngRoute', 'froala']);

    app.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

        $locationProvider.html5Mode(true);

        $routeProvider
            .when('/browse', {
                controller: 'BlogsController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/blogs.html'
            })
            .when('/users/:userId', {
                controller: 'UserProfileController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/user-profile.html'
            })
            .when('/users/:userId/posts', {
                controller: 'PostsController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/posts.html'
            })
            .when('/users/:userId/blogs', {
                controller: 'BlogsController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/blogs.html'
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
            .when('/users/:userId/new-blog', {
                controller: 'BlogCreateController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/blog-form.html'
            })
            .when('/users/:userId/blogs/:blogId/new-post', {
                controller: 'PostCreateController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/post-form.html'
            })
            .when('/users/:userId/blogs/:blogId/posts/:postId', {
                controller: 'PostController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/post.html'
            })
            .when('/users/:userId/blogs/:blogId/posts/:postId/edit', {
                controller: 'PostEditController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/post-form.html'
            })
            .when('/register', {
                controller: 'RegisterController',
                controllerAs: 'vm',
                templateUrl: '/app/src/templates/register.html'
            })
            .when('/unauthorized', {
                templateUrl: '/app/src/templates/unauthorized.html'
            })
            .when('/error', {
                templateUrl: '/app/src/templates/error.html'
            })
            .otherwise('/browse');
    }]);

})();