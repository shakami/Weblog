(function () {

    'use strict';

    angular
        .module('app')
        .controller('PostController', PostController);

    PostController.$inject = ['dataService', '$location'];

    function PostController(dataService, $location) {
        var vm = this;

        vm.post = {};
        vm.comments = [];

        vm.error = null;

        activate();

        function activate() {
            var path = $location.path();

            getPost(path);
        }

        function getPost(path) {
            dataService.getPost(path)
                .then(function (post) {
                    vm.post = post.data;
                    vm.comments = post.comments;
                })
                .catch(function (reason) {
                    vm.error = reason;
                });
        }
    }

})();