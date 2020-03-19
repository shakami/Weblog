(function () {

    'use strict';

    angular
        .module('app')
        .controller('BrowseController', BrowseController);

    BrowseController.$inject = ['dataService', '$location', '$routeParams', 'notifierService'];

    function BrowseController(dataService, $location, $routeParams, notifierService) {
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

            getBlogs(userId, pageNumber, pageSize);
        }

        function getBlogs(userId, pageNumber, pageSize) {
            dataService.getBlogs(userId, pageNumber, pageSize)
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