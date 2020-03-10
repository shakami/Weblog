(function () {

    'use strict';

    angular
        .module('app')
        .constant('API_URL', 'https://localhost:5001/api/')
        .factory('dataService', ['$http', '$q', 'API_URL', dataService]);

    function dataService($http, $q, API_URL) {
        return {
            getAllBlogs: getAllBlogs
        };

        function getAllBlogs() {
            var req =
            {
                url: API_URL + 'blogs',
                method: 'GET',
                headers: { 'Accept': 'application/json' }
            };

            return $http(req)
                .then(sendResponseData)
                .catch(sendError);
        }

        function sendResponseData(response) {
            return response.data;
        }

        function sendError(response) {
            return $q.reject('Status Code: ' + response.status);
        }
    }

})();