(function () {

    'use strict';

    angular
        .module('app')
        .controller('BlogsController', BlogsController);

    BlogsController.$inject = ['dataService'];

    function BlogsController(dataService) {
        var vm = this;

        vm.error = 'none';
        vm.data = [];
        dataService.getAllBlogs()
            .then(function (data) {
                vm.data = data;
            })
            .catch(function (reason) {
                vm.error = reason;
            });
    }

})();