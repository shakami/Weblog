(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlValidationErrors', wlValidationErrors);

    function wlValidationErrors() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/forms/wl-validation-errors.html'
        };
    }

})();