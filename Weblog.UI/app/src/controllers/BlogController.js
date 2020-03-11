(function () {

    'use strict';

    angular
        .module('app')
        .controller('BlogController', BlogController);

    BlogController.$inject = ['dataService', '$location'];

    function BlogController(dataService, $location) {
        var vm = this;

        vm.path = $location.path();
        
        dataService.getBlog(vm.path)
            .then(function (result) {
                vm.blog = result.data;
                vm.posts = result.posts;
            })
            .catch(function (reason) {
                vm.error = reason;
            });

        vm.getLinkForPost = function (post) {
            var apiLink = post.links[0].href;
            return (apiLink).substr(apiLink.lastIndexOf("users"));
        };
    }

})();