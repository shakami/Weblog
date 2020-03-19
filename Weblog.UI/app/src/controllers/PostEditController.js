(function () {

    angular
        .module('app')
        .controller('PostEditController', PostEditController);

    PostEditController.$inject = ['$routeParams', '$window', '$scope', 'dataService', 'textEditorService'];

    function PostEditController($routeParams, $window, $scope, dataService, textEditorService) {
        var vm = this;

        vm.editorOptions = textEditorService.postOptions();

        vm.title = null;
        vm.content = null;

        vm.submit = submit;
        vm.cancel = cancel;

        activate();

        function activate() {
            vm.userId = $routeParams.userId;
            vm.blogId = $routeParams.blogId;
            vm.postId = $routeParams.postId;

            if (!userAuthorized(vm.userId)) {
                $window.location.href = '/unauthorized';
            }

            getPost(vm.userId, vm.blogId, vm.postId);

            $scope.$on('$locationChangeStart', function (e) {
                var form = $scope.postForm;
                if (form.$dirty) {
                    var answer = confirm("Are you sure you want to leave this page? Your data won't be saved.");

                    if (!answer) {
                        e.preventDefault();
                    }
                }
            });
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

        function getPost(userId, blogId, postId) {
            dataService.getPost(userId, blogId, postId)
                .then(function (response) {
                    var post = response.data;
                    vm.title = post.title;
                    vm.content = post.body;
                })
                .catch(function (reason) {
                    console.log(reason);
                });
        }

        function submit(form) {
            if (form.$valid) {
                var post = {
                    title: vm.title,
                    body: vm.content
                };
                editPost(vm.userId, vm.blogId, vm.postId, post);
            }
        }

        function editPost(userId, blogId, postId, post) {
            var credentials = {
                emailAddress: $window.localStorage.getItem('email'),
                password: $window.localStorage.getItem('password')
            };

            dataService.editPost(userId, blogId, postId, post, credentials)
                .then(function () {
                    $window.location.href = "users/" + userId + '/blogs/' + blogId + '/posts/' + postId;
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