(function () {

    'use strict';

    angular
        .module('app')
        .controller('BlogController', BlogController);

    BlogController.$inject = ['dataService', '$location', '$routeParams', 'notifierService'];

    function BlogController(dataService, $location, $routeParams, notifierService) {
        var vm = this;

        vm.blog = {};
        vm.posts = [];
        vm.dataResolved = false;

        // used to handle paging
        vm.currentUrl = $location.path();
        vm.pageInfo = {};

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
                    vm.blog = response.data.blog;
                    vm.posts = response.data.posts;
                    vm.pageInfo = response.pagingHeader;

                    // get the name for the blog author
                    dataService.getUser(userId)
                        .then(function (response) {
                            vm.blog.userName = response.data.name;
                        })
                        .catch(function (reason) {
                            notifierService.error("Status Code: " + reason.status);
                            $location.path("/error");
                        });
                })
                .catch(function (reason) {
                    notifierService.error("Status Code: " + reason.status);
                    $location.path("/error");
                });
        }
    }

})();