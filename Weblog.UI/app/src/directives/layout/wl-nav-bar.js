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

                $scope.closeDropdown = function () {
                    $('#navbarDropdown').click();
                }

                $scope.login = function (form) {
                    if (form.$valid) {
                        dataService.authenticate($scope.emailAddress, $scope.password)
                            .then(function (data) {
                                $scope.loggedIn = true;
                                $scope.userName = data.name;

                                var links = data.links;
                                $scope.userBlogsLink = hateoasService.getBlogsByUserLink(links);
                                console.log($scope.userBlogsLink);
                            })
                            .catch(function (reason) {
                                $scope.error = reason
                            });
                    }
                }
            }
        };
    }

})();