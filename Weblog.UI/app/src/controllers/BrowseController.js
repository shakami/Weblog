(function () {

    'use strict';

    angular
        .module('app')
        .controller('BrowseController', BrowseController);

    BrowseController.$inject = ['dataService', '$location', '$routeParams', 'notifierService'];

    function BrowseController(dataService, $location, $routeParams, notifierService) {
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

            var userId = $routeParams.userId;

            getBlogs(userId, pageNumber, pageSize)
                .then(function (blogs) {
                    angular.forEach(blogs, function (blog, key) {
                        dataService.getUser(blog.userId)
                            .then(function (response) {
                                blog.userName = response.data.name;
                            })
                            .catch(function (reason) {
                                console.log(reason);
                            });
                    });
                })
                .catch(function (reason) {
                    console.log(reason);
                });
        }

        function getBlogs(userId, pageNumber, pageSize) {
            return dataService.getBlogs(userId, pageNumber, pageSize)
                .then(function (response) {
                    vm.blogs = response.data.blogs;
                    vm.pageInfo = response.pagingHeader;
                    return vm.blogs;
                })
                .catch(function (reason) {
                    notifierService.error("Status Code: " + reason.status);
                });
        }

        function getUsersForBlogs(blogs) {
            angular.forEach(blogs, function (blog, key) {
                console.log(blog);
            });
        }

        function getLinkForBlog(blog) {
            var apiLink = blog.links[0].href;
            return (apiLink).substr(apiLink.lastIndexOf("users"));
        };
    }

})();