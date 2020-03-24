(function () {

    'use strict';

    angular
        .module('app')
        .controller('BlogCreateController', BlogCreateController);

    BlogCreateController.$inject = ['$location', 'dataService', 'notifierService', 'userService'];

    function BlogCreateController($location, dataService, notifierService, userService) {
        var vm = this;

        vm.title = null;
        vm.excerpt = null;

        vm.submit = submit;

        vm.errors = null;

        activate();

        function activate() {
            vm.userId = userService.loggedInUser();

            if (!vm.userId) {
                // not logged in
                $location.path('/unauthorized');
            }
        }

        function submit(form) {
            if (form.$valid) {
                var blog = {
                    title: vm.title,
                    excerpt: vm.excerpt
                };

                createBlog(vm.userId, blog);
            }
        }

        function createBlog(userId, blog) {
            var credentials = userService.getCredentials();

            return dataService.createBlog(userId, blog, credentials)
                .then(function (response) {
                    var newBlogId = response.data.blogId;

                    notifierService.success();
                    $location.path('/users/' + userId + '/blogs/' + newBlogId);
                })
                .catch(function (reason) {
                    vm.errors = reason;
                });
        }
    }

})();