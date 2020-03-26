(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlComment', wlComment);

    function wlComment() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/comments/wl-comment.html',
            scope: {
                comment: '='
            },
            controller: function ($scope, userService) {
                $scope.isOwner = function (comment) {
                    return comment.userId === parseInt(userService.loggedInUser());
                }

                $scope.submit = function () {
                    $scope.$emit('commentUpdatedEvent', { comment: $scope.comment });
                    $('.edit-collapse' + $scope.comment.commentId).collapse('toggle');
                }

                $scope.confirmDelete = function () {
                    $scope.$emit('commentDeletedEvent', { comment: $scope.comment });
                }
            }
        };
    }

})();
