(function () {

    'use strict';

    angular
        .module('app')
        .controller('BlogsController', BlogsController);

    BlogsController.$inject =
        ['dataService', '$location', '$routeParams', 'notifierService'];

    function BlogsController(dataService, $location, $routeParams, notifierService) {
        var vm = this;

        vm.dataResolved = false;
        vm.blogs = [];

        // used to handle paging
        vm.currentUrl = $location.path();
        vm.pageInfo = {};

        activate();

        function activate() {
            var pageSize = $location.search().pageSize;
            var pageNumber = $location.search().pageNumber;

            // userId is set when browsing blogs from one certain author
            var userId = $routeParams.userId;
            var searchPhrase = $location.search().q;
            if (searchPhrase) {
                vm.currentUrl += '?q=' + searchPhrase + '&';
            } else {
                vm.currentUrl += '?';
            }

            getBlogs(userId, pageNumber, pageSize, searchPhrase);
        }

        function getBlogs(userId, pageNumber, pageSize, searchPhrase) {
            dataService.getBlogs(userId, pageNumber, pageSize, searchPhrase)
                .then(function (response) {
                    vm.blogs = response.data.blogs;
                    vm.pageInfo = response.pagingHeader;

                    // grab author's name for each blog
                    angular.forEach(vm.blogs, function (blog, index) {
                        dataService.getUser(blog.userId)
                            .then(function (response) {
                                blog.userName = response.data.name;
                            })
                            .catch(function (reason) {
                                notifierService.error("Status Code: " + reason.status);
                                $location.path("/error");
                            });
                    });

                    // signal the view to display data
                    vm.dataResolved = true;
                })
                .catch(function (reason) {
                    notifierService.error("Status Code: " + reason.status);
                    $location.path("/error");
                });
        }
    }

})();