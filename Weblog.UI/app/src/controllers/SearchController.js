(function () {

    'use strict';

    angular
        .module('app')
        .controller('SearchController', SearchController);

    SearchController.$inject = ['$location', 'dataService'];

    function SearchController($location, dataService) {
        var vm = this;

        vm.searchPhrase = $location.search().q;

        activate();

        function activate() {
            dataService.search(vm.searchPhrase)
                .then(function (response) {
                    console.log(response);
                })
                .catch(function (reason) {
                    console.log(reason);
                });
        }
    }

})();