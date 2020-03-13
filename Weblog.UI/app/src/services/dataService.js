(function () {

    'use strict';

    angular
        .module('app')
        .constant('API_URL', 'https://localhost:5001/api')
        .factory('dataService', ['$http', '$q', 'API_URL', dataService]);

    function dataService($http, $q, API_URL) {
        return {
            authenticate: authenticate,

            getBlogs: getBlogs,
            getBlog: getBlog,

            getPosts: getPosts,
            getPost: getPost,

            getComments: getComments
        };

        function authenticate(emailAddress, password) {
            var req =
            {
                url: API_URL + "/users/authenticate",
                method: 'POST',
                headers:
                {
                    'Accept': 'application/vnd.sepehr.hateoas+json',
                    'Content-Type': 'application/json'
                },
                data: { emailAddress, password }
            };

            return $http(req)
                .then(sendResponseData)
                .catch(sendError);
        }

        function getBlogs(userId, pageNumber, pageSize) {
            var paging = "?pageNumber=" + (pageNumber ?? "1") + "&pageSize=" + (pageSize ?? "9")

            var url = API_URL;
            if (userId) {
                url += /users/ + userId;
            }
            url += '/blogs' + paging;

            var req =
            {
                url: url,
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

        function getPost(path) {
            var req =
            {
                url: API_URL + path,
                method: 'GET',
                headers: { 'Accept': 'application/vnd.sepehr.hateoas+json' }
            };

            return $http(req)
                .then(function (data) {
                    return getComments(path + '/comments')
                        .then(function (result) {
                            data.comments = result.comments;
                            return data;
                        })
                        .catch(sendError);
                })
                .catch(sendError);
        }

        function getComments(path) {
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
            return {
                data: response.data,
                pagingHeader: angular.fromJson(response.headers('X-Pagination'))
            };
        }

        function sendError(response) {
            return $q.reject('Status Code: ' + response.status);
        }
    }

})();