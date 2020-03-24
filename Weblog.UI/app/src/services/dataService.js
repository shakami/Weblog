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
            editPost: editPost,
            deletePost: deletePost,

            getComments: getComments,
            createComment: createComment,
            editComment: editComment,
            deleteComment: deleteComment,

            searchBlogs: searchBlogs,
            searchUserPosts: searchUserPosts
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

        function getBlogs(userId, pageNumber, pageSize, searchQuery) {
            var params = "?searchQuery=" + (searchQuery ?? "") +
                "&pageNumber=" + (pageNumber ?? "1") + "&pageSize=" + (pageSize ?? "12");

            var url = API_URL;
            if (userId) {
                url += '/users/' + userId;
            }
            url += '/blogs' + params;

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
                    return getPostsForBlog(postsUrl)
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

        function getPostsForBlog(url) {
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

        function getPosts(userId, pageNumber, pageSize, searchQuery) {
            var params = "?searchQuery=" + (searchQuery ?? "") +
                "&pageNumber=" + (pageNumber ?? "1") + "&pageSize=" + (pageSize ?? "12");

            var url = API_URL + '/users/' + userId + '/posts' + params;

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

        function editPost(userId, blogId, postId, post, credentials) {
            post.credentials = credentials;
            var req =
            {
                url: API_URL + '/users/' + userId + '/blogs/' + blogId + '/posts/' + postId,
                method: 'PUT',
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

        function deletePost(userId, blogId, postId, credentials) {
            var req =
            {
                url: API_URL + '/users/' + userId + '/blogs/' + blogId + '/posts/' + postId,
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

        function editComment(userId, blogId, postId, commentId, comment, credentials) {
            comment.credentials = credentials;

            var url = API_URL +
                '/users/' + userId +
                '/blogs/' + blogId +
                '/posts/' + postId +
                '/comments/' + commentId;

            var req =
            {
                url: url,
                method: 'PUT',
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

        function deleteComment(userId, blogId, postId, commentId, credentials) {
            var url = API_URL +
                '/users/' + userId +
                '/blogs/' + blogId +
                '/posts/' + postId +
                '/comments/' + commentId;

            var req =
            {
                url: url,
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

        function searchBlogs(query, pageNumber, pageSize) {
           var paging = "&pageNumber=" + (pageNumber ?? "1") + "&pageSize=" + (pageSize ?? "12")

            var blogsUrl = API_URL + '/blogs/?searchQuery=' + query + paging;
            var blogsReq =
            {
                url: blogsUrl,
                method: 'GET',
                headers: { 'Accept': 'application/vnd.sepehr.hateoas+json' }
            };

            return $http(blogsReq)
                .then(sendResponseData)
                .catch(sendError);
        }

        function searchUserPosts(userId, query, pageNumber, pageSize) {
            var paging = "&pageNumber=" + (pageNumber ?? "1") + "&pageSize=" + (pageSize ?? "12")

            var postsUrl = API_URL + '/users/' + userId + '/posts/?searchQuery=' + query + paging;
            var postsReq =
            {
                url: postsUrl,
                method: 'GET',
                headers: { 'Accept': 'application/vnd.sepehr.hateoas+json' }
            };

            return $http(postsReq)
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