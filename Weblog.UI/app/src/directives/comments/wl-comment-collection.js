(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlCommentCollection', wlCommentCollection);

    function wlCommentCollection() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/comments/wl-comment-collection.html',
            scope: {
                comments: '='
            }
        };
    }

})();