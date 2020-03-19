(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlPostCollection', wlPostCollection);

    function wlPostCollection() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/posts/wl-post-collection.html',
            scope: {
                userId: '@',
                posts: '='
            },
            controller: function ($scope, $window) {
                $scope.isOwner = function () {
                    return $scope.userId === $window.localStorage.getItem('activeUserId');
                }

                $scope.postExcerpt = function (text) {
                    var html = '<div>' + text + '</div>';
                    html = String(html).replace(/<\//g, ' <\/')
                    return angular.element(html).text().substr(0, 250) + '...';
                }
            }
        };
    }

})();