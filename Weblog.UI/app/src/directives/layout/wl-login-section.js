(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlLoginSection', wlLoginSection);

    function wlLoginSection() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/layout/wl-login-section.html',
            replace: true,
            controller: function ($scope, userService, notifierService) {
                $scope.loggedIn = false;

                $scope.emailAddress = null;
                $scope.password = null;

                $scope.userName = null;

                $scope.userBlogsLink = "";
                $scope.userPostsLink = "";
                $scope.userProfileLink = "";

                $scope.login = login;
                $scope.logout = logout;

                activate();

                function activate() {
                    if (userService.loggedIn()) {
                        var credentials = userService.getCredentials();
                        userLogin(credentials.emailAddress, credentials.password);
                    }

                    $scope.$on('userUpdateEvent', function (e, args) {
                        e.stopPropagation();
                        $scope.userName = args.userName;
                    });

                    $scope.$on('userRegisteredEvent', function (e, args) {
                        e.stopPropagation();

                        notifierService.info('Account successfully created.');

                        userLogin(args.emailAddress, args.password);
                    });
                }

                function userLogin(emailAddress, password) {
                    userService.authenticate(emailAddress, password)
                        .then(function (response) {
                            var user = response.data;
                            $scope.loggedIn = true;
                            $scope.userName = user.name;

                            $scope.userProfileLink = '/users/' + user.userId;
                            $scope.userBlogsLink = $scope.userProfileLink + '/blogs';
                            $scope.userPostsLink = $scope.userProfileLink + '/posts';

                            $scope.$broadcast('loggedInEvent', { userId: user.userId });
                        })
                        .catch(function (reason) {
                            notifierService.warning(reason);
                        });
                }

                function login(form) {
                    if (form.$valid) {
                        userLogin($scope.emailAddress, $scope.password);

                        form.$setPristine();
                    }
                }

                function logout() {
                    $scope.loggedIn = false;

                    userService.logout();

                    $scope.$broadcast('loggedOutEvent');
                }
            }
        };
    }

})();