(function () {

    'use strict';

    angular
        .module('app')
        .controller('BlogController', BlogController);

    BlogController.$inject = ['dataService', '$location', '$routeParams'];

    function BlogController(dataService, $location, $routeParams) {
        var vm = this;

        vm.blog = {};
        vm.posts = [];

        vm.currentUrl = $location.path();
        vm.pageInfo = {};

        vm.error = null;

        vm.getLinkForPost = getLinkForPost;

        activate();

        function activate() {
            var pageSize = $location.search().pageSize;
            var pageNumber = $location.search().pageNumber;

            var userId = $routeParams.userId;
            var blogId = $routeParams.blogId;

            getBlog(userId, blogId, pageNumber, pageSize);
        }

        function getBlog(userId, blogId, pageNumber, pageSize) {
            dataService.getBlogWithPosts(userId, blogId, pageNumber, pageSize)
                .then(function (response) {
                    console.log(response);
                    vm.blog = response.data.blog;
                    vm.posts = response.data.posts;
                    vm.pageInfo = response.pagingHeader;
                })
                .catch(function (reason) {
                    vm.error = reason;
                });
        }

        function getLinkForPost(post) {
            var apiLink = post.links[0].href;
            return (apiLink).substr(apiLink.lastIndexOf("users"));
        };
    }

})();