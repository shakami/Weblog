(function () {

    'use strict';

    angular
        .module('app')
        .controller('BlogsController', BlogsController);

    BlogsController.$inject = ['dataService', '$location'];

    function BlogsController(dataService, $location) {
        var vm = this;

        vm.pageSize = $location.search().pageSize;
        vm.pageNumber = $location.search().pageNumber;

        dataService.getAllBlogs(vm.pageNumber, vm.pageSize)
            .then(function (data) {
                vm.data = data.blogs;
                vm.links = data.links;

                vm.hasNextPage = vm.links.find(e => e.rel === "nextPage");
                vm.hasPreviousPage = vm.links.find(e => e.rel === "previousPage");

                if (vm.hasNextPage) {
                    var apiLink = vm.hasNextPage.href;
                    console.log(apiLink);
                    vm.nextPageLink = (apiLink).substr(apiLink.lastIndexOf("blogs"));
                }

                if (vm.hasPreviousPage) {
                    var apiLink = vm.hasPreviousPage.href;
                    vm.previousPageLink = (apiLink).substr(apiLink.lastIndexOf("blogs"));
                }
            })
            .catch(function (reason) {
                vm.error = reason;
            });

        

        vm.getLinkForBlog = function (blog) {
            var apiLink = blog.links[0].href;
            return (apiLink).substr(apiLink.lastIndexOf("users"));
        };
    }

})();