(function () {

    'use strict';

    angular
        .module('app')
        .factory('routeService', ['$window', '$location', '$routeParams', routeService]);

    function routeService($window, $location, $routeParams) {
        return {
            goTo: goTo,
            goBack: goBack,
            getCurrentPath: getCurrentPath,
            getParam: getParam,
            getSearchParam: getSearchParam
        };

        function goTo(path) {
            $location.path(path);
        }

        function goBack() {
            $window.history.back();
        }

        function getCurrentPath() {
            return $location.path();
        }

        function getParam(param) {
            return $routeParams[param];
        }

        function getSearchParam(param) {
            return $location.search()[param];
        }
    }

})();