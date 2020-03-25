(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlLoginSection', wlLoginSection);

    function wlLoginSection() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/login/wl-login-section.html',
            replace: true,
            controller: function ($scope, userService, notifierService) {
                $scope.loggedIn = false;

                $scope.emailAddress = null;
                $scope.password = null;

                $scope.userName = null;

                $scope.error = null;

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

                    $scope.$on('userDeletedEvent', function (e, args) {
                        e.stopPropagation();

                        notifierService.info('Account successfully deleted.');

                        logout();
                    });
                }

                function userLogin(emailAddress, password) {
                    userService.authenticate(emailAddress, password)
                        .then(function (response) {
                            $scope.error = null;

                            var user = response.data;
                            $scope.loggedIn = true;
                            $scope.userName = user.name;

                            $scope.userProfileLink = '/users/' + user.userId;
                            $scope.userBlogsLink = $scope.userProfileLink + '/blogs';
                            $scope.userPostsLink = $scope.userProfileLink + '/posts';

                            $scope.$broadcast('loggedInEvent', { userId: user.userId });
                        })
                        .catch(function (reason) {
                            if (reason.status === 401) {
                                $scope.error = 'That did not match our records... Do you need to register?';
                            } else {
                                notifierService.error('Status Code: ' + reason.status);
                            }
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