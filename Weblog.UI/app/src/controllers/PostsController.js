(function () {

    'use strict';

    angular
        .module('app')
        .controller('PostsController', PostsController);

    PostsController.$inject = ['$routeParams', '$location', 'dataService', 'notifierService'];

    function PostsController($routeParams, $location, dataService, notifierService) {
        var vm = this;

        vm.dataResolved = false;

        vm.pageInfo = {};
        vm.currentUrl = $location.path();

        vm.userId = $routeParams.userId;

        vm.blogs = [];
        vm.posts = [];

        activate();

        function activate() {
            var pageSize = $location.search().pageSize;
            var pageNumber = $location.search().pageNumber;

            var searchPhrase = $location.search().q;

            if (searchPhrase) {
                vm.currentUrl += '?q=' + searchPhrase + '&';
            } else {
                vm.currentUrl += '?';
            }

            getPosts(vm.userId, pageNumber, pageSize, searchPhrase);
        }

        function getPosts(userId, pageNumber, pageSize, searchPhrase) {
            dataService.getPosts(userId, pageNumber, pageSize, searchPhrase)
                .then(function (response) {
                    vm.posts = response.data.posts;
                    vm.pageInfo = response.pagingHeader;
                    vm.dataResolved = true;
                })
                .catch(function (reason) {
                    notifierService.error('Status Code: ' + reason.status);
                    $location.path('/error');
                });
        }
    }

})();