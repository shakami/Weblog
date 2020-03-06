using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weblog.API.Entities;

namespace Weblog.API.Services
{
    interface IWeblogDataRepository
    {
        //-- users
        IEnumerable<User> GetUsers();

        User GetUser(int userId, bool includeBlogs);

        bool UserExists(int userId);

        void AddUser(User newUser);

        void UpdateUser(User updatedUser);

        void DeleteUser(User user);

        //-- blogs
        IEnumerable<Blog> GetBlogs(int userId);
        
        Blog GetBlog(int blogId);

        void AddBlog(int userId, Blog newBlog);

        void UpdateBlog(Blog updatedBlog);

        void DeleteBlog(Blog blog);

        //-- posts
        IEnumerable<Post> GetPosts(int blogId);

        Post GetPost(int postId);

        void AddPost(int blogId, Post newPost);

        void UpdatePost(Post updatedPost);

        void DeletePost(Post post);

        //-- comments
        IEnumerable<Comment> GetComments(int postId);

        void AddComment(int postId, Comment newComment);
        
        void UpdateComment(Comment updatedComment);

        void DeleteComment(Comment comment);

        //--save
        bool Save();
    }
}
