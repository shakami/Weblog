(function () {

    'use strict';

    angular
        .module('app')
        .controller('BlogCreateController', BlogCreateController);

    BlogCreateController.$inject = ['$window', 'dataService'];

    function BlogCreateController($window, dataService) {
        var vm = this;

        vm.title = null;
        vm.excerpt = null;

        vm.submit = submit;
        vm.cancel = cancel;

        activate();

        function activate() {
            vm.userId = $window.localStorage.getItem('activeUserId');

            if (!vm.userId) {
                $window.location.href = '/unauthorized';
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
            var credentials = {
                emailAddress: $window.localStorage.getItem('email'),
                password: $window.localStorage.getItem('password')
            };

            return dataService.createBlog(userId, blog, credentials)
                .then(function (response) {
                    var newBlogId = response.data.blogId;

                    $window.location.href = '/users/' + vm.userId + '/blogs/' + newBlogId;
                })
                .catch(function (reason) {
                    console.log(reason);
                });
        }

        function cancel() {
            $window.history.back();
        }
    }

})();