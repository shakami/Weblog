(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlBlogCollection', wlBlogCollection);

    function wlBlogCollection() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/blogs/wl-blog-collection.html',
            scope: {
                blogs: '='
            },
            controller: function ($scope, $window, $routeParams, dataService) {
                $scope.user = $window.localStorage.getItem('activeUserId');

                $scope.$on('loggedInEvent', function (e, args) {
                    $scope.user = args.userId;
                });

                $scope.$on('loggedOutEvent', function () {
                    $scope.user = null;
                });

                $scope.$on('blogDeleteEvent', function (e, args) {
                    var credentials = {
                        emailAddress: $window.localStorage.getItem('email'),
                        password: $window.localStorage.getItem('password')
                    };

                    dataService.deleteBlog(args.userId, args.blogId, credentials)
                        .then(function () {
                            $window.location.reload();
                        })
                        .catch(function (reason) {
                            console.log(reason);
                        });
                });

                $scope.isCurrentUserBlogs = $scope.user === $routeParams.userId;
            }
        };

    }

})();