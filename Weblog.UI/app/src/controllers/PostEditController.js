(function () {

    angular
        .module('app')
        .controller('PostEditController', PostEditController);

    PostEditController.$inject =
        ['$location', '$routeParams', 'userService', 'dataService', 'notifierService'];

    function PostEditController(
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
            vm.postId = $routeParams.postId;

            if (!userService.userAuthorized(vm.userId)) {
                $location.path('/unauthorized');
            }

            getPost(vm.userId, vm.blogId, vm.postId);
        }

        function getPost(userId, blogId, postId) {
            dataService.getPost(userId, blogId, postId)
                .then(function (response) {
                    var post = response.data;
                    vm.title = post.title;
                    vm.content = post.body;
                })
                .catch(function (reason) {
                    notifierService.error("Status Code: " + reason.status);
                    $location.path("/error");
                });
        }

        function submit(form) {
            if (form.$valid) {
                form.$setPristine();
                var post = {
                    title: vm.title,
                    body: vm.content
                };
                editPost(vm.userId, vm.blogId, vm.postId, post);
            }
        }

        function editPost(userId, blogId, postId, post) {
            var credentials = userService.getCredentials();

            dataService.editPost(userId, blogId, postId, post, credentials)
                .then(function () {
                    notifierService.success();
                    $location.path(
                        'users/' + userId +
                        '/blogs/' + blogId +
                        '/posts/' + postId);
                })
                .catch(function (reason) {
                    vm.errors = reason;
                });
        }
    }

})();
