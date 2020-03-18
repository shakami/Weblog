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
            controller: function ($scope, dataService) {
                $scope.dataResolved = false;
                $scope.comments = [];

                $scope.commenting = false;
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