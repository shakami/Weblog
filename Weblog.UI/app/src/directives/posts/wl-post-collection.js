(function () {

    'use strict';

    angular
        .module('app')
        .directive('wlPostCollection', wlPostCollection);

    function wlPostCollection() {
        return {
            restrict: 'E',
            templateUrl: 'app/src/directives/posts/wl-post-collection.html',
            scope: {
                userId: '@',
                posts: '='
            },
            controller: function ($scope, userService, dataService, notifierService) {
                $scope.isOwner = function () {
                    return $scope.userId === userService.loggedInUser();
                }

                $scope.postExcerpt = function (text) {
                    var html = '<div>' + text + '</div>';
                    html = String(html).replace(/<\//g, ' <\/')
                    return angular.element(html).text().substr(0, 250) + '...';
                }

                $scope.confirmDelete = function (post) {
                    var credentials = userService.getCredentials();

                    dataService.deletePost($scope.userId, post.blogId, post.postId, credentials)
                        .then(function () {
                            var index = $scope.posts.indexOf(post);
                            $scope.posts.splice(index, 1);
                            notifierService.success();
                        })
                        .catch(function (reason) {
                            notifierService.warning(reason);
                        });
                }
            }
        };
    }

})();
