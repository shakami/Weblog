(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlCommentSection', wlCommentSection);

    function wlCommentSection() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/comments/wl-comment-section.html',
            scope: {
                userId: '@',
                blogId: '@',
                postId: '@'
            },
            controller: function ($scope, dataService, textEditorService, $window) {
                $scope.editorOptions = textEditorService.commentOptions();

                $scope.dataResolved = false;
                $scope.comments = [];
                $scope.commentCount = null;

                $scope.newComment = null;
                $scope.commenting = false;
                $scope.toggleComment = toggleComment;
                $scope.addComment = addComment;
                $scope.submit = submit;

                activate();

                function activate() {
                    getComments($scope.userId, $scope.blogId, $scope.postId);
                }

                function getComments(userId, blogId, postId) {
                    dataService.getComments(userId, blogId, postId)
                        .then(function (response) {
                            $scope.comments = response.data.comments;
                            console.log(response.pagingHeader);
                            $scope.commentCount = response.pagingHeader.totalCount;

                            var totalPages = response.pagingHeader.totalPages;
                            for (var pageNumber = 2; pageNumber <= totalPages; pageNumber++) {
                                dataService.getComments(userId, blogId, postId, pageNumber)
                                    .then(function (response) {
                                        angular.forEach(response.data.comments, function (comment, key) {
                                            $scope.comments.push(comment);
                                        });
                                    })
                                    .catch(function (reason) {
                                        console.log(reason);
                                    });
                            }
                            
                            // get the author name for each comment
                            angular.forEach($scope.comments, function (comment, key) {
                                dataService.getUser(comment.userId)
                                    .then(function (response) {
                                        comment.userName = response.data.name;
                                    })
                                    .catch(function (reason) {
                                        console.log(reason);
                                    });
                            });

                            $scope.dataResolved = true;
                        })
                        .catch(function (reason) {
                            console.log(reason);
                        });
                }

                function toggleComment() {
                    $scope.commenting = !$scope.commenting;
                }

                function addComment() {
                    if ($window.localStorage.getItem('activeUserId')) {
                        $('[data-toggle="popover"]').popover('dispose');
                        toggleComment();
                    } else {
                        $('[data-toggle="popover"]').popover('toggle');
                    }
                }

                function submit(form) {
                    if (form.$dirty) {

                        var comment = {
                            userId: parseInt($window.localStorage.getItem('activeUserId')),
                            body: $scope.newComment
                        };
                        createComment($scope.userId, $scope.blogId, $scope.postId, comment);
                    }
                }

                function createComment(userId, blogId, postId, comment) {
                    var credentials = {
                        emailAddress: $window.localStorage.getItem('email'),
                        password: $window.localStorage.getItem('password')
                    };
                    dataService.createComment(userId, blogId, postId, comment, credentials)
                        .then(function () {
                            $window.location.reload();
                        })
                        .catch(function (reason) {
                            console.log(reason);
                        });
                }
            }
        };
    }

})();