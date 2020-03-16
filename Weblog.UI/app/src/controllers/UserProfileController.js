(function () {

    'use strict';

    angular
        .module('app')
        .controller('UserProfileController', UserProfileController);

    UserProfileController.$inject = ['$routeParams', 'dataService', '$window', '$scope'];

    function UserProfileController($routeParams, dataService, $window, $scope) {
        var vm = this;

        vm.userName = null;
        vm.firstName = null;
        vm.lastName = null;
        vm.email = null;
        vm.password = null;
        vm.confirmPassword = null;

        vm.editing = false;

        vm.toggleEdit = toggleEdit;
        vm.save = save;

        activate();

        function activate() {
            vm.userId = $routeParams.userId;

            if (!userAuthorized(vm.userId)) {
                $window.location.href = '/unauthorized';
            }

            getUser(vm.userId);
        }

        function userAuthorized(userId) {
            var loggedInUser = $window.localStorage.getItem('activeUserId');
            if (!loggedInUser) {
                return false;
            }
            if (loggedInUser !== userId) {
                return false;
            }

            vm.password = $window.localStorage.getItem('password');
            return true;
        }

        function getUser(userId) {
            dataService.getUser(userId)
                .then(function (response) {
                    vm.userName = response.data.name;
                    vm.email = response.data.emailAddress;
                    var names = vm.userName.split(' ');
                    if (names.length === 2) {
                        vm.firstName = names[0].trim();
                        vm.lastName = names[1].trim();
                    }
                })
                .catch(function (reason) {
                    console.log(reason);
                });
        }

        function toggleEdit() {
            vm.editing = !vm.editing;
            if (vm.editing) {
                vm.emailForEdit = angular.copy(vm.email);
                vm.passwordForEdit = angular.copy(vm.password);
                vm.confirmPassword = angular.copy(vm.password);
                vm.firstNameForEdit = angular.copy(vm.firstName);
                vm.lastNameForEdit = angular.copy(vm.lastName);
            }
        }

        function save(form) {
            if (form.$valid) {
                if (vm.passwordForEdit !== vm.confirmPassword) {
                    // passwords do not match
                    return;
                }

                vm.email = angular.copy(vm.emailForEdit);
                vm.password = angular.copy(vm.passwordForEdit);
                vm.firstName = angular.copy(vm.firstNameForEdit);
                vm.lastName = angular.copy(vm.lastNameForEdit);
                vm.userName = vm.firstName + ' ' + vm.lastName;

                // talk to server
                var user = {
                    firstName: vm.firstName,
                    lastName: vm.lastName,
                    emailAddress: vm.email,
                    password: vm.password
                };
                var credentials = {
                    emailAddress: $window.localStorage.getItem('email'),
                    password: $window.localStorage.getItem('password')
                };

                dataService.editUser(vm.userId, user, credentials)
                    .then(function () {
                        // update credentials
                        $window.localStorage.setItem('email', vm.email);
                        $window.localStorage.setItem('password', vm.password);

                        $scope.$emit('userUpdateEvent', { userName: vm.userName });
                    })
                    .catch(function (reason) {
                        console.log(reason);
                    });

                toggleEdit();
            }
        }
    }

})();