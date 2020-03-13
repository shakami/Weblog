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
            }
        };
    }

})();