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
            controller: function ($scope) {
                $scope.postExcerpt = function (text) {
                    var html = '<div>' + text + '</div>';
                    html = String(html).replace(/<\//g, ' <\/')
                    return angular.element(html).text().substr(0, 250) + '...';
                }
            }
        };
    }

})();