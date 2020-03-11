(function () {

    'use strict';

    angular
        .module('app')
        .controller('BlogController', BlogController);

    BlogController.$inject = ['dataService', '$location'];

    function BlogController(dataService, $location) {
        var vm = this;

        vm.blog = {};
        vm.posts = [];

        vm.error = null;

        vm.getLinkForPost = getLinkForPost;

        activate();

        function activate() {
            var path = $location.path();

            getBlog(path);
        }

        function getBlog(path) {
            dataService.getBlog(path)
                .then(function (result) {
                    vm.blog = result.data;
                    vm.posts = result.posts;
                })
                .catch(function (reason) {
                    vm.error = reason;
                });
        }

        function getLinkForPost(post) {
            var apiLink = post.links[0].href;
            return (apiLink).substr(apiLink.lastIndexOf("users"));
        };
    }

})();