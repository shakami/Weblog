﻿(function () {

    'use strict';

    angular
        .module('app')
        .controller('BrowseController', BrowseController);

    BrowseController.$inject = ['dataService', '$location', '$routeParams'];

    function BrowseController(dataService, $location, $routeParams) {
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

            getBlogs(userId, pageNumber, pageSize);
        }

        function getBlogs(userId, pageNumber, pageSize) {
            dataService.getBlogs(userId, pageNumber, pageSize)
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