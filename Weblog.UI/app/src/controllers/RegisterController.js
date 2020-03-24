(function () {

    'use strict';

    angular
        .module('app')
        .controller('RegisterController', RegisterController);

    RegisterController.$inject = ['dataService', '$location', '$scope'];

    function RegisterController(dataService, $location, $scope) {
        var vm = this;

        vm.firstName = "";
        vm.lastName = "";
        vm.email = "";
        vm.password = "";
        vm.confirmPassword = "";

        vm.register = register;
        vm.errors = null;

        function register(form) {
            if (form.$valid) {
                if (vm.password !== vm.confirmPassword) {
                    vm.errors = ["Make sure the passwords match!"];
                    return;
                }
                vm.errors = null;

                var user = {
                    firstName: vm.firstName,
                    lastName: vm.lastName,
                    emailAddress: vm.email,
                    password: vm.password
                };

                // make api call to create user
                dataService.register(user).then(function (response) {
                    $scope.$emit('userRegisteredEvent', user);
                    $location.path('/users/' + response.data.userId);
                }).catch(function (reason) {
                    if (reason[0][0].includes('duplicate')) {
                        vm.errors = ['There is already an account for this email address.'];
                    } else {
                        vm.errors = reason;
                    }
                })
            }
        }
    }

})();