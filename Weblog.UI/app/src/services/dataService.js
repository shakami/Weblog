(function () {

    'use strict';

    angular
        .module('app')
        .constant('API_URL', 'https://localhost:5001/api')
        .factory('dataService', ['$http', '$q', 'API_URL', dataService]);

    function dataService($http, $q, API_URL) {
        return {
            authenticate: authenticate,
            register: register,

            getBlogs: getBlogs,
            getBlog: getBlog,
            editBlog: editBlog,
            getBlogWithPosts: getBlogWithPosts,
            createBlog: createBlog,
            deleteBlog: deleteBlog,

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

        function register(user) {
            var req = {
                url: API_URL + '/users',
                method: 'POST',
                headers:
                {
                    'Accept': 'application/vnd.sepehr.hateoas+json',
                    'Content-Type': 'application/json'
                },
                data: user
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

        function getBlog(userId, blogId) {
            var req =
            {
                url: API_URL + '/users/' + userId + '/blogs/' + blogId,
                method: 'GET',
                headers: { 'Accept': 'application/vnd.sepehr.hateoas+json' }
            };

            return $http(req)
                .then(sendResponseData)
                .catch(sendError);
        }

        function editBlog(userId, blogId, blog, credentials) {
            blog.credentials = credentials;
            var req =
            {
                url: API_URL + '/users/' + userId + '/blogs/' + blogId,
                method: 'PUT',
                headers:
                {
                    'Accept': 'application/vnd.sepehr.hateoas+json',
                    'Content-Type': 'application/json'
                },
                data: blog
            }

            return $http(req)
                .then(sendResponseData)
                .catch(sendError);
        }

        function createBlog(userId, blog, credentials) {
            blog.credentials = credentials;

            var req =
            {
                url: API_URL + '/users/' + userId + '/blogs',
                method: 'POST',
                headers:
                {
                    'Accept': 'application/vnd.sepehr.hateoas+json',
                    'Content-Type': 'application/json'
                },
                data: blog
            }

            return $http(req)
                .then(sendResponseData)
                .catch(sendError);
        }

        function deleteBlog(userId, blogId, credentials) {
            var req =
            {
                url: API_URL + '/users/' + userId + '/blogs/' + blogId,
                method: 'DELETE',
                headers:
                {
                    'Accept': 'application/vnd.sepehr.hateoas+json',
                    'Content-Type': 'application/json'
                },
                data: credentials
            }

            return $http(req)
                .then(sendResponseData)
                .catch(sendError);
        }

        function getBlogWithPosts(path) {
            var req =
            {
                url: API_URL + path,
                method: 'GET',
                headers: { 'Accept': 'application/vnd.sepehr.hateoas+json' }
            };

            return $http(req)
                .then(function (blogResponse) {
                    return getPosts(path + '/posts')
                        .then(function (postsResponse) {
                            return {
                                data: {
                                    blog: blogResponse.data,
                                    posts: postsResponse.data.posts
                                },
                                pagingHeader: postsResponse.pagingHeader
                            };
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
            if (response.status === 422) {
                var errors = [];

                angular.forEach(response.data.errors, function (value, key) {
                    errors.push(value);
                });
                return $q.reject(errors);
            }
            return $q.reject(response);
        }
    }

})();