(function () {

    'use strict';

    angular
        .module('app')
        .controller('EditBlogController', EditBlogController);

    EditBlogController.$inject = ['dataService', '$routeParams', '$window'];

    function EditBlogController(dataService, $routeParams, $window) {
        var vm = this;

        vm.title = null;
        vm.excerpt = null;

        vm.submit = submit;

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
                    console.log(reason);
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
                    $window.history.back();
                })
                .catch(function (reason) {
                    console.log(reason);
                });
        }
    }

})();