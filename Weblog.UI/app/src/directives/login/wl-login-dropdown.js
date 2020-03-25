(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlLoginDropdown', wlLoginDropdown);

    function wlLoginDropdown() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/login/wl-login-dropdown.html',
            replace: true,
        };
    }

})();