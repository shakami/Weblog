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
            }
        };

    }

})();