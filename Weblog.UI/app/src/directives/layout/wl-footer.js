(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlFooter', wlFooter);

    function wlFooter() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/layout/wl-footer.html'
        };
    }

})();