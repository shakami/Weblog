(function () {

    'use strict';

    angular
        .module('app')
        .controller('UserProfileController', UserProfileController);

    UserProfileController.$inject = ['$location', '$routeParams', 'userService', 'dataService', '$scope', 'notifierService'];

    function UserProfileController($location, $routeParams, userService, dataService, $scope, notifierService) {
        var vm = this;

        vm.user = {};

        vm.errors = null;

        vm.confirmDeleteInput = "";
        vm.confirmDelete = confirmDelete;

        vm.isOwner = function () {
            return vm.user.userId === userService.loggedInUser();
        }

        activate();

        function activate() {
            vm.user.userId = $routeParams.userId;

            getUser(vm.user.userId);
        }

        function getUser(userId) {
            dataService.getUser(userId)
                .then(function (response) {
                    vm.user.userName = response.data.name;
                    vm.user.email = response.data.emailAddress;
                    var names = vm.user.userName.split(' ');
                    if (names.length === 2) {
                        vm.user.firstName = names[0].trim();
                        vm.user.lastName = names[1].trim();
                    }
                })
                .catch(function (reason) {
                    notifierService.error("Status Code: " + reason.status);
                    $location.path("/error");
                });
        }

        function confirmDelete(form) {
            if (form.$invalid) {
                return;
            }
            if (vm.confirmDeleteInput.trim().toUpperCase() !== 'confirm'.toUpperCase()) {
                notifierService.warning('You need to type the word "confirm".');
                return;
            }
            // delete user
            var credentials = userService.getCredentials();

            dataService.deleteUser(vm.user.userId, credentials)
                .then(function () {
                    $scope.$emit('userDeletedEvent');
                    $location.path('/');
                })
                .catch(function (reason) {
                    notifierService.warning(reason);
                });
        }
    }

})();