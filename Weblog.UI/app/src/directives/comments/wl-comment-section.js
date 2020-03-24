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
            controller: function ($scope, dataService, $window, userService, notifierService) {
                $scope.dataResolved = false;
                $scope.comments = [];
                $scope.commentCount = null;
                $scope.hasMoreComments = hasMoreComments;
                $scope.getMoreComments = getMoreComments;

                $scope.newComment = null;
                $scope.commenting = false;
                $scope.toggleComment = toggleComment;
                $scope.addComment = addComment;
                $scope.submit = submit;

                activate();

                function activate() {
                    getComments($scope.userId, $scope.blogId, $scope.postId);

                    $scope.$on('commentUpdatedEvent', function (event, args) {
                        event.stopPropagation();

                        editComment($scope.userId, $scope.blogId, $scope.postId,
                            args.comment.commentId, args.comment);
                    });

                    $scope.$on('commentDeletedEvent', function (event, args) {
                        event.stopPropagation();

                        deleteComment($scope.userId, $scope.blogId, $scope.postId,
                            args.comment.commentId, args.comment);
                    });
                }

                function getComments(userId, blogId, postId) {
                    dataService.getComments(userId, blogId, postId)
                        .then(function (response) {
                            $scope.comments = response.data.comments;
                            $scope.commentCount = parseInt(response.pagingHeader.totalCount);

                            // get the author name for each comment
                            angular.forEach($scope.comments, function (comment, key) {
                                dataService.getUser(comment.userId)
                                    .then(function (response) {
                                        comment.userName = response.data.name;
                                    })
                                    .catch(function (reason) {
                                        notifierService.error('Status Code: ' + reason.status);
                                    });
                            });

                            $scope.dataResolved = true;
                        })
                        .catch(function (reason) {
                            notifierService.error('Status Code: ' + reason.status);
                        });
                }

                function editComment(userId, blogId, postId, commentId, comment) {
                    var credentials = userService.getCredentials();

                    dataService.editComment(userId, blogId, postId, commentId, comment, credentials)
                        .then(function () {
                            notifierService.success();
                        })
                        .catch(function (reason) {
                            notifierService.warning(reason);
                        });
                }

                function deleteComment(userId, blogId, postId, commentId, comment) {
                    var credentials = userService.getCredentials();

                    dataService.deleteComment(userId, blogId, postId, commentId, credentials)
                        .then(function () {
                            var index = $scope.comments.indexOf(comment);
                            $scope.comments.splice(index, 1);
                            $scope.commentCount--;
                            notifierService.success();
                        })
                        .catch(function (reason) {
                            notifierService.warning(reason);
                        });
                }

                function hasMoreComments() {
                    return $scope.commentCount > $scope.comments.length
                }

                function getMoreComments() {
                    var pageNumber = ($scope.comments.length / 10) + 1;
                    dataService.getComments($scope.userId, $scope.blogId, $scope.postId, pageNumber)
                        .then(function (response) {
                            angular.forEach(response.data.comments, function (comment, key) {
                                // get the author name comment
                                dataService.getUser(comment.userId)
                                    .then(function (response) {
                                        comment.userName = response.data.name;
                                    })
                                    .catch(function (reason) {
                                        notifierService.error('Status Code: ' + reason.status);
                                    });

                                // add comment to the list
                                $scope.comments.push(comment);
                            });
                        })
                        .catch(function (reason) {
                            notifierService.error('Status Code: ' + reason.status);

                        });
                }

                function toggleComment() {
                    $scope.commenting = !$scope.commenting;
                }

                function addComment() {
                    if (userService.loggedIn()) {
                        $('[data-toggle="popover"]').popover('dispose');
                        toggleComment();
                    } else {
                        $('[data-toggle="popover"]').popover('toggle');
                    }
                }

                function submit(form) {
                    if (form.$dirty) {

                        var comment = {
                            userId: parseInt(userService.loggedInUser()),
                            body: $scope.newComment
                        };
                        createComment($scope.userId, $scope.blogId, $scope.postId, comment);
                    }
                }

                function createComment(userId, blogId, postId, comment) {
                    var credentials = userService.getCredentials();

                    dataService.createComment(userId, blogId, postId, comment, credentials)
                        .then(function () {
                            notifierService.success();
                            $window.location.reload();
                        })
                        .catch(function (reason) {
                            notifierService.warning(reason);
                        });
                }
            }
        };
    }

})();