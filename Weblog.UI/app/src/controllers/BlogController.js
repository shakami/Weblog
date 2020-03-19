(function () {

    'use strict';

    angular
        .module('app')
        .controller('BlogController', BlogController);

    BlogController.$inject = ['dataService', '$window', '$location', '$routeParams', 'notifierService'];

    function BlogController(dataService, $window, $location, $routeParams, notifierService) {
        var vm = this;

        vm.owner = false;

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

            vm.owner = userId === $window.localStorage.getItem('activeUserId');

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

                            // signal the view to display data
                            vm.dataResolved = true;
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