(function () {

    'use strict';

    angular
        .module('app')
        .controller('BrowseController', BrowseController);

    BrowseController.$inject = ['dataService', '$location'];

    function BrowseController(dataService, $location) {
        var vm = this;

        vm.blogs = [];
        vm.error = null;

        vm.currentUrl = $location.path();
        vm.pageInfo = {};

        vm.getLinkForBlog = getLinkForBlog;

        activate();

        function activate() {
            var pageSize = $location.search().pageSize;
            var pageNumber = $location.search().pageNumber;

            getAllBlogs(pageNumber, pageSize);
        }

        function getAllBlogs(pageNumber, pageSize) {
            dataService.getAllBlogs(pageNumber, pageSize)
                .then(function (response) {
                    vm.blogs = response.data.blogs;
                    vm.pageInfo = response.pagingHeader;
                })
                .catch(function (reason) {
                    vm.error = reason;
                });
        }

        

        function getLinkForBlog(blog) {
            var apiLink = blog.links[0].href;
            return (apiLink).substr(apiLink.lastIndexOf("users"));
        };
    }

})();