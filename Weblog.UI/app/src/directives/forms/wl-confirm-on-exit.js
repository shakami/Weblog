(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlConfirmOnExit', wlConfirmOnExit);

    function wlConfirmOnExit() {
        return {
            restrict: 'A',
            link: function (scope, elem, attrs) {
                scope.$on('$locationChangeStart', function (event, next, current) {
                    if (elem.hasClass('ng-dirty')) {
                        var answer =
                            confirm("Are you sure you want to leave this page? Your data won't be saved.");

                        if (!answer) {
                            event.preventDefault();
                        }
                    }
                });
            }
        };
    }

})();