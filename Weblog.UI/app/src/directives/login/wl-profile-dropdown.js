(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlProfileDropdown', wlProfileDropdown);

    function wlProfileDropdown() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/login/wl-profile-dropdown.html',
            replace: true
        };
    }

})();