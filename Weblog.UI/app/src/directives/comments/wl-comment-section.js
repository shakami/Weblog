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

                $scope.commenting = false;

                $scope.addComment = function () {
                    if ($window.localStorage.getItem('activeUserId')) {
                        $('[data-toggle="popover"]').popover('dispose');
                        $scope.toggleComment();
                    } else {
                        $('[data-toggle="popover"]').popover('toggle');
                    }
                }

                $scope.toggleComment = function () {
                    $scope.commenting = !$scope.commenting;
                }

                activate();

                function activate() {
                    getComments($scope.userId, $scope.blogId, $scope.postId);
                }

                function getComments(userId, blogId, postId) {
                    dataService.getComments(userId, blogId, postId)
                        .then(function (response) {
                            $scope.comments = response.data.comments;

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
            }
        };
    }

})();