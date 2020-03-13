﻿(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlNavBar', wlNavBar);

    function wlNavBar() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/layout/wl-nav-bar.html',
            controller: function ($scope, dataService, $window) {
                
                $scope.loggedIn = false;

                $scope.emailAddress = null;
                $scope.password = null;

                $scope.userName = null;
                $scope.userBlogsLink = "";

                $scope.error = null;

                $scope.login = function (form) {
                    if (form.$valid) {
                        userLogin();
                    }
                }

                $scope.logout = function () {
                    $scope.loggedIn = false;

                    $window.localStorage.removeItem('activeUserId');
                    $window.localStorage.removeItem('email');
                    $window.localStorage.removeItem('password');

                    $scope.$broadcast('loggedOutEvent');
                }

                if ($window.localStorage.getItem('activeUserId')) {
                    $scope.emailAddress = $window.localStorage.getItem('email');
                    $scope.password = $window.localStorage.getItem('password');
                    userLogin();
                }

                function userLogin() {
                    dataService.authenticate($scope.emailAddress, $scope.password)
                        .then(function (response) {
                            var user = response.data;
                            $scope.loggedIn = true;
                            $scope.userName = user.name;

                            $scope.userBlogsLink = '/users/' + user.userId + '/blogs';

                            $window.localStorage.setItem('activeUserId', user.userId);
                            $window.localStorage.setItem('email', $scope.emailAddress);
                            $window.localStorage.setItem('password', $scope.password);

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
        };
    }

})();