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
            getUser: getUser,
            editUser: editUser,

            getBlogs: getBlogs,
            getBlog: getBlog,
            editBlog: editBlog,
            getBlogWithPosts: getBlogWithPosts,
            createBlog: createBlog,
            deleteBlog: deleteBlog,

            getPosts: getPosts,
            getPost: getPost,
            createPost: createPost,

            getComments: getComments,
            createComment: createComment
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

        function getUser(userId) {
            var req =
            {
                url: API_URL + '/users/' + userId,
                method: 'GET',
                headers: { 'Accept': 'application/vnd.sepehr.hateoas+json' }
            };

            return $http(req)
                .then(sendResponseData)
                .catch(sendError);
        }

        function editUser(userId, user, credentials) {
            user.credentials = credentials;

            var req = {
                url: API_URL + '/users/' + userId,
                method: 'PUT',
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

        function getBlogWithPosts(userId, blogId, pageNumber, pageSize) {
            var paging = "?pageNumber=" + (pageNumber ?? "1") + "&pageSize=" + (pageSize ?? "10")

            var blogUrl = API_URL + /users/ + userId + '/blogs/' + blogId;
            var postsUrl = blogUrl + '/posts' + paging;

            var req =
            {
                url: blogUrl,
                method: 'GET',
                headers: { 'Accept': 'application/vnd.sepehr.hateoas+json' }
            };

            return $http(req)
                .then(function (blogResponse) {
                    return getPosts(postsUrl)
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

        function getPosts(url) {
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

        function getPost(userId, blogId, postId) {
            var postUrl = API_URL + '/users/' + userId + '/blogs/' + blogId + '/posts/' + postId;
            var commentsUrl = postUrl + '/comments';

            var req =
            {
                url: postUrl,
                method: 'GET',
                headers: { 'Accept': 'application/vnd.sepehr.hateoas+json' }
            };

            return $http(req)
                .then(sendResponseData)
                .catch(sendError);
        }

        function createPost(userId, blogId, post, credentials) {
            post.credentials = credentials;

            var req =
            {
                url: API_URL + '/users/' + userId + '/blogs/' + blogId + '/posts',
                method: 'POST',
                headers:
                {
                    'Accept': 'application/vnd.sepehr.hateoas+json',
                    'Content-Type': 'application/json'
                },
                data: post
            }

            return $http(req)
                .then(sendResponseData)
                .catch(sendError);
        }

        function getComments(userId, blogId, postId, pageNumber) {
            var paging = "?pageNumber=" + (pageNumber ?? "1");

            var url = API_URL +
                '/users/' + userId +
                '/blogs/' + blogId +
                '/posts/' + postId +
                '/comments';

            var req =
            {
                url: url + paging,
                method: 'GET',
                headers: { 'Accept': 'application/vnd.sepehr.hateoas+json' }
            };

            return $http(req)
                .then(sendResponseData)
                .catch(sendError);
        }

        function createComment(userId, blogId, postId, comment, credentials) {
            comment.credentials = credentials;

            var url = API_URL +
                '/users/' + userId +
                '/blogs/' + blogId +
                '/posts/' + postId +
                '/comments'

            var req =
            {
                url: url,
                method: 'POST',
                headers:
                {
                    'Accept': 'application/vnd.sepehr.hateoas+json',
                    'Content-Type': 'application/json'
                },
                data: comment
            }

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