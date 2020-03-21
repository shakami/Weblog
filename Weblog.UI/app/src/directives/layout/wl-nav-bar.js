(function () {

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

                activate();

                function activate() {
                    if ($window.localStorage.getItem('activeUserId')) {
                        initialize();
                        userLogin();
                    }

                    $scope.$on('userUpdateEvent', function (e, args) {
                        e.stopPropagation();
                        $scope.userName = args.userName;
                    });

                    $scope.$on('userRegisteredEvent', function (e, args) {
                        e.stopPropagation();
                        $scope.emailAddress = args.emailAddress;
                        $scope.password = args.password;
                        userLogin();
                    });
                }

                function initialize() {
                    $scope.emailAddress = $window.localStorage.getItem('email');
                    $scope.password = $window.localStorage.getItem('password');
                }

                function userLogin() {
                    dataService.authenticate($scope.emailAddress, $scope.password)
                        .then(function (response) {
                            var user = response.data;
                            $scope.loggedIn = true;
                            $scope.userName = user.name;

                            $scope.userProfileLink = '/users/' + user.userId;
                            $scope.userBlogsLink = $scope.userProfileLink + '/blogs';

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