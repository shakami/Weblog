(function () {

    'use strict';

    angular
        .module('app')
        .controller('BlogEditController', BlogEditController);

    BlogEditController.$inject = ['dataService', '$routeParams', '$window', 'notifierService'];

    function BlogEditController(dataService, $routeParams, $window, notifierService) {
        var vm = this;

        vm.title = null;
        vm.excerpt = null;

        vm.submit = submit;
        vm.cancel = cancel;

        vm.errors = null;

        activate();

        function activate() {
            vm.userId = $routeParams.userId;
            vm.blogId = $routeParams.blogId;

            if (!userAuthorized(vm.userId)) {
                $window.location.href = '/unauthorized';
            }

            getBlog(vm.userId, vm.blogId);
        }

        function userAuthorized(userId) {
            var loggedInUser = $window.localStorage.getItem('activeUserId');
            if (!loggedInUser) {
                return false;
            }
            if (loggedInUser !== userId) {
                return false;
            }
            return true;
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
            var credentials = {
                emailAddress: $window.localStorage.getItem('email'),
                password: $window.localStorage.getItem('password')
            };

            dataService.editBlog(userId, blogId, blog, credentials)
                .then(function () {
                    notifierService.success();
                    $window.history.back();
                })
                .catch(function (reason) {
                    vm.errors = reason;
                });
        }

        function cancel() {
            $window.history.back();
        }
    }

})();