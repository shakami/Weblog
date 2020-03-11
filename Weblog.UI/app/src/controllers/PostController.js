(function () {

    'use strict';

    angular
        .module('app')
        .controller('PostController', PostController);

    PostController.$inject = ['dataService', '$location'];

    function PostController(dataService, $location) {
        var vm = this;

        vm.path = $location.path();

        dataService.getPost(vm.path)
            .then(function (post) {
                vm.post = post.data;
                vm.comments = post.comments;
            })
            .catch(function (reason) {
                vm.error = reason;
            });
    }

})();