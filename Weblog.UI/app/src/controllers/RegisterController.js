(function () {

    'use strict';

    angular
        .module('app')
        .controller('RegisterController', RegisterController);

    RegisterController.$inject = ['dataService'];

    function RegisterController(dataService) {
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

                // make api call to create user
                dataService.register({
                    firstName: vm.firstName,
                    lastName: vm.lastName,
                    emailAddress: vm.email,
                    password: vm.password
                }).then(function (data) {
                    console.log(data);
                    vm.errors = null;
                }).catch(function (reason) {
                    vm.errors = reason;
                })
            }
        }
    }

})();