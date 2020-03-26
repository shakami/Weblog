(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlCancelButton', wlCancelButton);

    function wlCancelButton() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/forms/wl-cancel-button.html',
            replace: true,
            controller: function ($scope, $window) {
                $scope.cancel = function () {
                    $window.history.back();
                }
            }
        };
    }

})();
