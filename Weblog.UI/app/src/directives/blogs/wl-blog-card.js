(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlBlogCard', wlBlogCard);

    function wlBlogCard() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'app/src/directives/blogs/wl-blog-card.html',
            scope: {
                blog: '='
            },
            controller: function ($scope, $window) {
                $scope.deleting = false;

                $scope.owner = $scope.blog.userId === parseInt($window.localStorage.getItem('activeUserId'));

                $scope.$on('loggedInEvent', function (e, args) {
                    $scope.owner = ($scope.blog.userId === args.userId);
                });

                $scope.$on('loggedOutEvent', function () {
                    $scope.owner = false;
                });

                $scope.toggleDelete = function () {
                    $scope.deleting = !$scope.deleting;
                };

                $scope.confirmDelete = function () {
                    $scope.$emit('blogDeleteEvent', { userId: $scope.blog.userId, blogId: $scope.blog.blogId });
                };
            }
        };
    }

})();