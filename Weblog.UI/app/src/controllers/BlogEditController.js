(function () {

    'use strict';

    angular
        .module('app')
        .controller('BlogEditController', BlogEditController);

    BlogEditController.$inject = ['$location', '$routeParams', 'dataService', 'userService', 'notifierService'];

    function BlogEditController($location, $routeParams, dataService, userService, notifierService) {
        var vm = this;

        vm.title = null;
        vm.excerpt = null;

        vm.submit = submit;

        vm.errors = null;

        activate();

        function activate() {
            vm.userId = $routeParams.userId;
            vm.blogId = $routeParams.blogId;

            if (!userService.userAuthorized(vm.userId)) {
                $location.path('/unauthorized');
            }

            getBlog(vm.userId, vm.blogId);
        }

        function getBlog(userId, blogId) {
            dataService.getBlog(userId, blogId)
                .then(function (response) {
                    var blog = response.data;
                    vm.title = blog.title;
                    vm.excerpt = blog.excerpt;
                })
                .catch(function (reason) {
                    notifierService.error("Status Code: " + reason.status);
                    $location.path("/error");
                });
        }

        function submit(form) {
            if (form.$valid) {
                var blog = {
                    title: vm.title,
                    excerpt: vm.excerpt
                };
                saveBlog(vm.userId, vm.blogId, blog);
            }
        }

        function saveBlog(userId, blogId, blog) {
            var credentials = userService.getCredentials();

            dataService.editBlog(userId, blogId, blog, credentials)
                .then(function () {
                    notifierService.success();
                    $location.path('/users/' + userId + '/blogs/' + blogId);
                })
                .catch(function (reason) {
                    vm.errors = reason;
                });
        }
    }

})();