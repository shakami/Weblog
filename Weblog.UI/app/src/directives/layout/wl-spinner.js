(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlSpinner', wlSpinner);

    function wlSpinner() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/layout/wl-spinner.html'
        };
    }

})();
