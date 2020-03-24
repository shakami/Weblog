(function () {

    'use strict';

    angular
        .module('app')
        .controller('PostCreateController', PostCreateController);

    PostCreateController.$inject = ['$routeParams', '$window', 'dataService', '$scope', 'textEditorService', 'notifierService'];

    function PostCreateController($routeParams, $window, dataService, $scope, textEditorService, notifierService) {
        var vm = this;

        vm.editorOptions = textEditorService.postOptions();

        vm.title = null;
        vm.content = null;

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

        function submit(form) {
            if (vm.content && form.$valid) {
                var post = {
                    title: vm.title,
                    body: vm.content
                };
                createPost(vm.userId, vm.blogId, post);
            }
        }

        function createPost(userId, blogId, post) {
            var credentials = {
                emailAddress: $window.localStorage.getItem('email'),
                password: $window.localStorage.getItem('password')
            };

            dataService.createPost(userId, blogId, post, credentials)
                .then(function (response) {
                    var postId = response.data.postId;
                    notifierService.success();
                    $window.location.href = "users/" + userId + '/blogs/' + blogId + '/posts/' + postId;
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