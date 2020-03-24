(function () {

    'use strict';

    angular
        .module('app')
        .controller('UserProfileController', UserProfileController);

    UserProfileController.$inject = ['$location', '$routeParams', 'userService', 'dataService', '$scope', 'notifierService'];

    function UserProfileController($location, $routeParams, userService, dataService, $scope, notifierService) {
        var vm = this;

        vm.userId = null;
        vm.userName = null;
        vm.firstName = null;
        vm.lastName = null;
        vm.email = null;
        vm.password = null;
        vm.confirmPassword = null;

        vm.editing = false;

        vm.errors = null;

        vm.toggleEdit = toggleEdit;
        vm.save = save;

        activate();

        function activate() {
            vm.userId = $routeParams.userId;

            if (!userService.userAuthorized(vm.userId)) {
                $location.path('/unauthorized');
            }

            getUser(vm.userId);
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
                    notifierService.error("Status Code: " + reason.status);
                    $location.path("/error");
                });
        }

        function toggleEdit() {
            vm.editing = !vm.editing;
            if (vm.editing) {
                resetData();
            }
        }

        function save(form) {
            if (form.$valid) {
                if (vm.passwordForEdit !== vm.confirmPassword) {
                    // passwords do not match
                    return;
                }

                persistData();

                // talk to server
                var user = {
                    firstName: vm.firstName,
                    lastName: vm.lastName,
                    emailAddress: vm.email,
                    password: vm.password
                };

                editUser(vm.userId, user);
            }
        }

        function editUser(userId, user) {
            var credentials = userService.getCredentials();

            dataService.editUser(userId, user, credentials)
                .then(function () {
                    // update credentials
                    userService.setCredentials(user.emailAddress, user.password);
                    $scope.$emit('userUpdateEvent', { userName: vm.userName });

                    notifierService.success();
                    toggleEdit();
                })
                .catch(function (reason) {
                    if (reason[0][0].includes('duplicate')) {
                        vm.errors = ['There is already an account for this email address.'];
                    } else {
                        vm.errors = reason;
                    }
                });
        }

        function persistData() {
            vm.email = angular.copy(vm.emailForEdit);
            vm.password = angular.copy(vm.passwordForEdit);
            vm.firstName = angular.copy(vm.firstNameForEdit);
            vm.lastName = angular.copy(vm.lastNameForEdit);
            vm.userName = vm.firstName + ' ' + vm.lastName;
        }

        function resetData() {
            vm.emailForEdit = angular.copy(vm.email);
            vm.passwordForEdit = angular.copy(vm.password);
            vm.confirmPassword = angular.copy(vm.password);
            vm.firstNameForEdit = angular.copy(vm.firstName);
            vm.lastNameForEdit = angular.copy(vm.lastName);
        }
    }

})();