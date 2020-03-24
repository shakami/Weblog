(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlProfileDropdown', wlProfileDropdown);

    function wlProfileDropdown() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/layout/wl-profile-dropdown.html',
            replace: true
        };
    }

})();