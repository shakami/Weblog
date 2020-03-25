(function () {

    'use strict';

    angular
        .module('app')
        .controller('BlogController', BlogController);

    BlogController.$inject = ['$routeParams', '$location', 'dataService', 'userService', 'notifierService'];

    function BlogController($routeParams, $location, dataService, userService, notifierService) {
        var vm = this;

        vm.isOwner = isOwner;

        vm.blog = {};
        vm.posts = [];
        vm.dataResolved = false;

        // used to handle paging
        vm.currentUrl = $location.path() + '?';
        vm.pageInfo = {};

        activate();

        function activate() {
            var pageSize = $location.search().pageSize;
            var pageNumber = $location.search().pageNumber;

            vm.userId = $routeParams.userId;
            vm.blogId = $routeParams.blogId;

            getBlog(vm.userId, vm.blogId, pageNumber, pageSize);
        }

        function isOwner() {
            return vm.userId === userService.loggedInUser();   
        };

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