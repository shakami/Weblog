(function () {

    'use strict';

    angular
        .module('app')
        .value('froalaConfig', {
            placeholderText: 'Body',
            toolbarButtons: {
                'moreText': {
                    'buttons': ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', 'textColor', 'backgroundColor', 'clearFormatting'],
                    'buttonsVisible': 2
                },
                'moreParagraph': {
                    'buttons': ['formatOLSimple', 'alignLeft', 'alignCenter', 'alignRight', 'alignJustify', 'formatOL', 'formatUL', 'paragraphFormat', 'paragraphStyle', 'lineHeight', 'outdent', 'indent', 'quote'],
                    'buttonsVisible': 1
                },
                'moreRich': {
                    'buttons': ['insertLink', 'emoticons', 'insertHR', 'insertImage', 'insertVideo', 'insertTable', 'specialCharacters']
                },
                'moreMisc': {
                    'buttons': ['undo', 'redo', 'fullscreen', 'print', 'getPDF', 'spellChecker', 'selectAll', 'help'],
                    'align': 'right',
                    'buttonsVisible': 2
                }
            },
            quickInsertEnabled: false,
            imagePaste: false,
            imageUpload: false,
            videoUpload: false,
            fileUpload: false
        })
        .controller('NewPostController', NewPostController);

    NewPostController.$inject = ['$routeParams', '$window', 'dataService', '$scope'];

    function NewPostController($routeParams, $window, dataService, $scope) {
        var vm = this;

        vm.title = null;
        vm.content = null;

        vm.submit = submit;

        activate();

        function activate() {
            vm.userId = $routeParams.userId;
            vm.blogId = $routeParams.blogId;

            if (!userAuthorized(vm.userId)) {
                $window.location.href = '/unauthorized';
            }

            $scope.$on('$locationChangeStart', function (e) {
                var form = $scope.newPostForm;
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
                    $window.location.href = "users/" + userId + '/blogs/' + blogId + '/posts/' + postId;
                })
                .catch(function (reason) {
                    console.log(reason);
                });
        }
    }

})();