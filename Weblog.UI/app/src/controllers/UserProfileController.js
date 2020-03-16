(function () {

    'use strict';

    angular
        .module('app')
        .controller('UserProfileController', UserProfileController);

    UserProfileController.$inject = ['$routeParams', 'dataService'];

    function UserProfileController($routeParams, dataService) {
        var vm = this;

        vm.userName = null;
        vm.email = null;

        activate();

        function activate() {
            var userId = $routeParams.userId;
            getUser(userId);
        }

        function getUser(userId) {
            dataService.getUser(userId)
                .then(function (response) {
                    vm.userName = response.data.name;
                    vm.email = response.data.emailAddress;
                })
                .catch(function (reason) {
                    console.log(reason);
                });
        }
    }

})();