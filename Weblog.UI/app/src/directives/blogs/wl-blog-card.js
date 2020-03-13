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
            }
        };
    }

})();