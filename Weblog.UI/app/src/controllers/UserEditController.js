(function () {

    'use strict';

    angular
        .module('app')
        .controller('UserEditController', UserEditController);

    UserEditController.$inject =
        ['$scope', '$routeParams', '$location', 'userService', 'dataService', 'notifierService'];
    function UserEditController(
        $scope, $routeParams, $location, userService, dataService, notifierService) {
        var vm = this;

        vm.userId = null;

        vm.firstName = null;
        vm.lastName = null;
        vm.email = null;
        vm.password = null;
        vm.confirmPassword = null;

        vm.submit = submit;

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
                    vm.email = response.data.emailAddress;

                    var userName = response.data.name;
                    var names = userName.split(' ');
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

        function submit(form) {
            if (form.$invalid) {
                return;
            }

            if (vm.password !== vm.confirmPassword) {
                // passwords do not match
                vm.errors = ['Make sure the passwords match!']
                return;
            }

            var user = {
                firstName: vm.firstName,
                lastName: vm.lastName,
                emailAddress: vm.email,
                password: vm.password
            };

            // talk to server
            editUser(vm.userId, user);
        }

        function editUser(userId, user) {
            var credentials = userService.getCredentials();

            dataService.editUser(userId, user, credentials)
                .then(function () {
                    // update credentials
                    userService.setCredentials(user.emailAddress, user.password);
                    $scope.$emit('userUpdateEvent', { userName: vm.userName });

                    notifierService.success();
                    $location.path('/users/' + userId);
                })
                .catch(function (reason) {
                    if (reason[0][0].includes('duplicate')) {
                        vm.errors = ['There is already an account for this email address.'];
                    } else {
                        vm.errors = reason;
                    }
                });
        }
    }

})();
