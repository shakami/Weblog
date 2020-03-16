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
            controller: function ($scope, $window) {
                $scope.user = $window.localStorage.getItem('activeUserId');

                $scope.$on('loggedInEvent', function (e, args) {
                    $scope.user = args.userId;
                });

                $scope.$on('loggedOutEvent', function () {
                    $scope.user = null;
                });
            }
        };

    }

})();