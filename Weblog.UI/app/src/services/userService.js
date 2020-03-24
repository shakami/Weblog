(function () {

    'use strict';

    angular
        .module('app')
        .factory('userService', ['$window', 'dataService', userService]);

    function userService($window, dataService) {
        return {
            loggedIn: loggedInUser,
            loggedInUser: loggedInUser,

            getCredentials: getCredentials,
            setCredentials: setCredentials,
            userAuthorized: userAuthorized,

            authenticate: authenticate,
            logout: logout
        };

        function loggedInUser() {
            return $window.localStorage.getItem('activeUserId');
        }

        function getCredentials() {
            return {
                emailAddress: $window.localStorage.getItem('email'),
                password: $window.localStorage.getItem('password')
            };
        }

        function setCredentials(emailAddress, password) {
            $window.localStorage.setItem('email', emailAddress);
            $window.localStorage.setItem('password', password);
        }

        function userAuthorized(userId) {
            var activeUser = loggedInUser();
            if (!activeUser) {
                return false;
            }
            if (activeUser !== userId) {
                return false;
            }
            return true;
        }

        function authenticate(emailAddress, password) {
            return dataService.authenticate(emailAddress, password)
                .then(function (response) {
                    var user = response.data;

                    $window.localStorage.setItem('activeUserId', user.userId);
                    setCredentials(emailAddress, password);

                    return response;
                })
                .catch(function (reason) {
                    return reason;
                });
        }

        function logout() {
            $window.localStorage.removeItem('activeUserId');
            $window.localStorage.removeItem('email');
            $window.localStorage.removeItem('password');
        }
    }

})();