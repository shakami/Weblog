(function () {

    'use strict';

    angular
        .module('app')
        .controller('BlogsController', BlogsController);

    BlogsController.$inject = ['dataService'];

    function BlogsController(dataService) {
        var vm = this;

        dataService.getAllBlogs()
            .then(function (data) {
                vm.data = data.blogs;
                vm.links = data.links;
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