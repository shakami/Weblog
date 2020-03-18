﻿(function () {

    'use strict';

    angular
        .module('app')
        .controller('PostController', PostController);

    PostController.$inject = ['dataService', '$routeParams'];

    function PostController(dataService, $routeParams) {
        var vm = this;

        vm.userId = null;
        vm.blogId = null;
        vm.postId = null;

        vm.post = {};
        vm.dataResolved = false;

        activate();

        function activate() {
            vm.userId = $routeParams.userId;
            vm.blogId = $routeParams.blogId;
            vm.postId = $routeParams.postId;

            getPost(vm.userId, vm.blogId, vm.postId);
        }

        function getPost(userId, blogId, postId) {
            dataService.getPost(userId, blogId, postId)
                .then(function (response) {
                    vm.post = response.data;

                    vm.post.userId = userId;
                    // get author name for the post
                    dataService.getUser(vm.post.userId)
                        .then(function (response) {
                            vm.post.userName = response.data.name;
                        })
                        .catch(function (reason) {
                            console.log(reason);
                        });

                    vm.dataResolved = true;
                })
                .catch(function (reason) {
                    vm.error = reason;
                });
        }

    }

})();