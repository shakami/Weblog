(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlNavBar', wlNavBar);

    function wlNavBar() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/layout/wl-nav-bar.html',
            controller: function ($scope, dataService, $rootScope) {
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

                                $scope.userBlogsLink = '/users/' + user.userId + '/blogs';

                                $rootScope.activeUserId = user.userId;

                                $scope.$broadcast('loggedInEvent', { userId: user.userId });

                                form.$setPristine();
                                $scope.emailAddress = null;
                                $scope.password = null;
                            })
                            .catch(function (reason) {
                                $scope.error = reason
                            });
                    }
                }

                $scope.logout = function () {
                    $scope.loggedIn = false;
                    $rootScope.activeUserId = null;
                    $scope.$broadcast('loggedOutEvent');
                }
            }
        };
    }

})();