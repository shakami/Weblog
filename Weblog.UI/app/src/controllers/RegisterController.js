(function () {

    'use strict';

    angular
        .module('app')
        .controller('RegisterController', RegisterController);

    RegisterController.$inject = [];

    function RegisterController() {
        var vm = this;

        vm.firstName = "";
        vm.lastName = "";
        vm.email = "";
        vm.password = "";
        vm.confirmPassword = "";

        vm.register = register;
        vm.error = null;

        function register(form) {
            if (form.$valid) {
                if (vm.password !== vm.confirmPassword) {
                    vm.error = "Make sure the passwords match!";
                    return;
                }
                vm.error = null;

                // make api call to create user
            }
        }
    }

})();