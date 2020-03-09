using System.Collections.Generic;
using Weblog.API.Entities;
using Weblog.API.Helpers;
using Weblog.API.ResourceParameters;

namespace Weblog.API.Services
{
    public interface IWeblogDataRepository
    {
        //--------- users ---------//
        PagedList<User> GetUsers(UsersResourceParameters resourceParameters);

        User GetUser(int userId, bool includeBlogs);

        bool UserExists(int userId);

        void AddUser(User newUser);

        void UpdateUser(User updatedUser);

        void DeleteUser(User user);

        //--------- blogs ---------//
        PagedList<Blog> GetBlogs(BlogsResourceParameters resourceParameters);

        PagedList<Blog> GetBlogs(int userId, BlogsResourceParameters resourceParameters);
        
        Blog GetBlog(int blogId, bool includePosts);

        bool BlogExists(int blogId);

        void AddBlog(Blog newBlog);

        void UpdateBlog(Blog updatedBlog);

        void DeleteBlog(Blog blog);

        //--------- posts ---------//
        PagedList<Post> GetPosts(int blogId, PostsResourceParameters resourceParameters);

        Post GetPost(int postId, bool includeComments);

        bool PostExists(int postId);

        void AddPost(int blogId, Post newPost);

        void UpdatePost(Post updatedPost);
        
        void DeletePost(Post post);

        //--------- comments ---------//
        PagedList<Comment> GetComments(int postId, CommentsResourceParameters resourceParameters);

        Comment GetComment(int commentId);

        void AddComment(int postId, Comment newComment);
        
        void UpdateComment(Comment updatedComment);

        void DeleteComment(Comment comment);

        //--------- save ---------//
        bool Save();
    }
}
