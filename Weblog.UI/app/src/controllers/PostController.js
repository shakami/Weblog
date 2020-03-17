(function () {

    'use strict';

    angular
        .module('app')
        .controller('PostController', PostController);

    PostController.$inject = ['dataService', '$routeParams'];

    function PostController(dataService, $routeParams) {
        var vm = this;

        vm.post = {};
        vm.comments = [];

        vm.error = null;

        activate();

        function activate() {
            var userId = $routeParams.userId;
            var blogId = $routeParams.blogId;
            var postId = $routeParams.postId;
            // var path = $location.path();

            getPost(userId, blogId, postId);
        }

        function getPost(userId, blogId, postId) {
            dataService.getPost(userId, blogId, postId)
                .then(function (response) {
                    vm.post = response.data.post;
                    vm.comments = response.data.comments;
                })
                .catch(function (reason) {
                    vm.error = reason;
                });
        }
    }

})();