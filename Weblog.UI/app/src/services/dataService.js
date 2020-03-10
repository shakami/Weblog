(function () {

    'use strict';

    angular
        .module('app')
        .constant('API_URL', 'https://localhost:5001/api')
        .factory('dataService', ['$http', '$q', 'API_URL', dataService]);

    function dataService($http, $q, API_URL) {
        return {
            getAllBlogs: getAllBlogs,
            getBlog: getBlog,

            getPosts: getPosts
        };

        function getAllBlogs() {
            var req =
            {
                url: API_URL + '/blogs',
                method: 'GET',
                headers: { 'Accept': 'application/vnd.sepehr.hateoas+json' }
            };

            return $http(req)
                .then(sendResponseData)
                .catch(sendError);
        }

        function getBlog(path) {
            var req =
            {
                url: API_URL + path,
                method: 'GET',
                headers: { 'Accept': 'application/vnd.sepehr.hateoas+json' }
            };

            return $http(req)
                .then(function (data) {
                    return getPosts(path + '/posts')
                        .then(function (result) {
                            data.posts = result.posts;
                            return data;
                        })
                        .catch(sendError);
                })
                .catch(sendError);
        }

        function getPosts(path) {
            var req =
            {
                url: API_URL + path,
                method: 'GET',
                headers: { 'Accept': 'application/vnd.sepehr.hateoas+json' }
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