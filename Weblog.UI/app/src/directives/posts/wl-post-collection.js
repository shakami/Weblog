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
            controller: function ($scope, $window, dataService) {
                $scope.isOwner = function () {
                    return $scope.userId === $window.localStorage.getItem('activeUserId');
                }

                $scope.postExcerpt = function (text) {
                    var html = '<div>' + text + '</div>';
                    html = String(html).replace(/<\//g, ' <\/')
                    return angular.element(html).text().substr(0, 250) + '...';
                }

                $scope.confirmDelete = function (post) {
                    var credentials = {
                        emailAddress: $window.localStorage.getItem('email'),
                        password: $window.localStorage.getItem('password')
                    };
                    dataService.deletePost($scope.userId, post.blogId, post.postId, credentials)
                        .then(function () {
                            var index = $scope.posts.indexOf(post);
                            $scope.posts.splice(index, 1);
                        })
                        .catch(function (reason) {
                            console.log(reason);
                        });
                }
            }
        };
    }

})();