(function () {

    'use strict';

    angular
        .module('app')
        .controller('PostCreateController', PostCreateController);

    PostCreateController.$inject =
        ['$location', '$routeParams', 'userService', 'dataService', 'notifierService'];

    function PostCreateController(
        $location, $routeParams, userService, dataService, notifierService) {
        var vm = this;

        vm.title = null;
        vm.content = null;

        vm.submit = submit;

        vm.errors = null;

        activate();

        function activate() {
            vm.userId = $routeParams.userId;
            vm.blogId = $routeParams.blogId;

            if (!userService.userAuthorized(vm.userId)) {
                $location.path('/unauthorized');
            }
        }

        function submit(form) {
            if (vm.content && form.$valid) {
                form.$setPristine();
                var post = {
                    title: vm.title,
                    body: vm.content
                };
                createPost(vm.userId, vm.blogId, post);
            }
        }

        function createPost(userId, blogId, post) {
            var credentials = userService.getCredentials();

            dataService.createPost(userId, blogId, post, credentials)
                .then(function (response) {
                    var postId = response.data.postId;
                    notifierService.success();
                    $location.path("users/" + userId + '/blogs/' + blogId + '/posts/' + postId);
                })
                .catch(function (reason) {
                    vm.errors = reason;
                });
        }
    }

})();
