(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlNavBar', wlNavBar);

    function wlNavBar() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/layout/wl-nav-bar.html',
            controller: function ($scope, dataService, hateoasService) {
                $scope.loggedIn = false;

                $scope.emailAddress = null;
                $scope.password = null;

                $scope.userName = null;
                $scope.userBlogsLink = "";

                $scope.error = null;

                $scope.login = function (form) {
                    if (form.$valid) {
                        dataService.authenticate($scope.emailAddress, $scope.password)
                            .then(function (response) {
                                var user = response.data;
                                $scope.loggedIn = true;
                                $scope.userName = user.name;

                                var links = user.links;
                                $scope.userBlogsLink = hateoasService.getBlogsByUserLink(links);
                            })
                            .catch(function (reason) {
                                $scope.error = reason
                            });
                    }
                }

                $scope.logout = function () {
                    $scope.loggedIn = false;
                }
            }
        };
    }

})();