(function () {

    'use strict';

    angular
        .module('app')
        .controller('BlogsController', BlogsController);

    BlogsController.$inject = ['dataService', '$location'];

    function BlogsController(dataService, $location) {
        var vm = this;

        vm.data = [];
        vm.error = null;

        vm.nextPageLink = null;
        vm.previousPageLink = null;

        vm.getLinkForBlog = getLinkForBlog;

        activate();

        function activate() {
            var pageSize = $location.search().pageSize;
            var pageNumber = $location.search().pageNumber;

            getAllBlogs(pageNumber, pageSize);
        }

        function getAllBlogs(pageNumber, pageSize) {
            dataService.getAllBlogs(pageNumber, pageSize)
                .then(function (data) {
                    vm.data = data.blogs;
                    generatePagingLinks(data.links);
                })
                .catch(function (reason) {
                    vm.error = reason;
                });
        }

        function generatePagingLinks(links) {
            var hasNextPage = links.find(e => e.rel === "nextPage");
            var hasPreviousPage = links.find(e => e.rel === "previousPage");

            if (hasNextPage) {
                var apiLink = hasNextPage.href;
                vm.nextPageLink = (apiLink).substr(apiLink.lastIndexOf("blogs"));
            }

            if (hasPreviousPage) {
                var apiLink = hasPreviousPage.href;
                vm.previousPageLink = (apiLink).substr(apiLink.lastIndexOf("blogs"));
            }
        }

        function getLinkForBlog(blog) {
            var apiLink = blog.links[0].href;
            return (apiLink).substr(apiLink.lastIndexOf("users"));
        };
    }

})();