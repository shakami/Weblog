using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Weblog.API.DbContexts;
using Weblog.API.Entities;
using Weblog.API.Helpers;
using Weblog.API.ResourceParameters;

namespace Weblog.API.Services
{
    public class WeblogDataRepository : IWeblogDataRepository
    {
        private readonly WeblogContext _context;

        public WeblogDataRepository(WeblogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //--------- users ---------//
        #region users
        public PagedList<User> GetUsers(UsersResourceParameters resourceParameters)
        {
            if (resourceParameters is null)
            {
                throw new ArgumentNullException(nameof(resourceParameters));
            }

            var collection = _context.Users
                                .OrderBy(u => u.LastName)
                                .ThenBy(u => u.FirstName);

            return PagedList<User>.Create(collection,
                                          resourceParameters.PageNumber,
                                          resourceParameters.PageSize);
        }

        public User GetUser(int userId)
        {
            return _context.Users
                .Where(u => u.UserId == userId)
                .FirstOrDefault();
        }

        public bool UserExists(int userId)
        {
            return _context.Users
                .Any(u => u.UserId == userId);
        }

        public void AddUser(User newUser)
        {
            _context.Users.Add(newUser);
        }

        public void UpdateUser(User updatedUser)
        {
            // no code
        }

        public void DeleteUser(User user)
        {
            var comments = _context.Comments
                .Where(c => c.UserId == user.UserId);

            _context.Comments.RemoveRange(comments);

            _context.Users.Remove(user);
        }
        #endregion

        //--------- blogs ---------//
        #region blogs
        public PagedList<Blog> GetBlogs(BlogsResourceParameters resourceParameters)
        {
            if (resourceParameters is null)
            {
                throw new ArgumentNullException(nameof(resourceParameters));
            }

            var collection = _context.Blogs as IQueryable<Blog>;

            var searchQuery = resourceParameters.SearchQuery?.Trim();

            if (searchQuery != null)
            {
                collection = collection
                    .Where(b => b.Title.Contains(searchQuery));
            }

            return PagedList<Blog>.Create(collection,
                                          resourceParameters.PageNumber,
                                          resourceParameters.PageSize);
        }

        public PagedList<Blog> GetBlogs(int userId, BlogsResourceParameters resourceParameters)
        {
            if (resourceParameters is null)
            {
                throw new ArgumentNullException(nameof(resourceParameters));
            }

            var collection = _context.Blogs
                .Where(b => b.UserId == userId) as IQueryable<Blog>;

            var searchQuery = resourceParameters.SearchQuery?.Trim();

            if (searchQuery != null)
            {
                collection = collection
                    .Where(b => b.Title.Contains(searchQuery));
            }

            return PagedList<Blog>.Create(collection,
                                          resourceParameters.PageNumber,
                                          resourceParameters.PageSize);
        }

        public Blog GetBlog(int blogId)
        {
            return _context.Blogs
                .Where(b => b.BlogId == blogId)
                .FirstOrDefault();
        }

        public bool BlogExists(int blogId)
        {
            return _context.Blogs
                .Any(b => b.BlogId == blogId);
        }

        public void AddBlog(int userId, Blog newBlog)
        {
            var user = GetUser(userId);

            user.Blogs.Add(newBlog);
        }

        public void UpdateBlog(Blog updatedBlog)
        {
            // no code
        }

        public void DeleteBlog(Blog blog)
        {
            _context.Blogs.Remove(blog);
        }
        #endregion

        //--------- posts ---------//
        #region posts
        public PagedList<Post> GetPosts(PostsResourceParameters resourceParameters)
        {
            if (resourceParameters is null)
            {
                throw new ArgumentNullException(nameof(resourceParameters));
            }

            var collection = _context.Posts as IQueryable<Post>;

            var searchQuery = resourceParameters.SearchQuery?.Trim();

            if (searchQuery != null)
            {
                collection = collection.Where(p => p.Title.Contains(searchQuery));
            }

            collection = collection
                            .OrderByDescending(p => p.TimeCreated);

            return PagedList<Post>.Create(collection,
                                          resourceParameters.PageNumber,
                                          resourceParameters.PageSize);
        }

        public PagedList<Post> GetPosts(int blogId, PostsResourceParameters resourceParameters)
        {
            if (resourceParameters is null)
            {
                throw new ArgumentNullException(nameof(resourceParameters));
            }

            var collection = _context.Posts
                .Where(p => p.BlogId == blogId);

            var searchQuery = resourceParameters.SearchQuery?.Trim();

            if (searchQuery != null)
            {
                collection = collection.Where(p => p.Title.Contains(searchQuery));
            }

            collection = collection
                            .OrderByDescending(p => p.TimeCreated);

            return PagedList<Post>.Create(collection,
                                          resourceParameters.PageNumber,
                                          resourceParameters.PageSize);
        }

        public Post GetPost(int postId)
        {
            return _context.Posts
                .Where(p => p.PostId == postId)
                .FirstOrDefault();
        }

        public bool PostExists(int postId)
        {
            return _context.Posts
                .Any(p => p.PostId == postId);
        }

        public void AddPost(int blogId, Post newPost)
        {
            var blog = GetBlog(blogId);

            blog.Posts.Add(newPost);
        }

        public void UpdatePost(Post updatedPost)
        {
            // no code
        }

        public void DeletePost(Post post)
        {
            _context.Posts.Remove(post);
        }
        #endregion

        //-------- comments -------//
        #region comments
        public PagedList<Comment> GetComments(int postId, CommentsResourceParameters resourceParameters)
        {
            if (resourceParameters is null)
            {
                throw new ArgumentNullException(nameof(resourceParameters));
            }

            var collection = _context.Comments
                                .Where(c => c.PostId == postId)
                                .OrderByDescending(c => c.TimeCreated);

            return PagedList<Comment>.Create(collection,
                                             resourceParameters.PageNumber,
                                             resourceParameters.PageSize);
        }

        public Comment GetComment(int commentId)
        {
            return _context.Comments
                .Where(c => c.CommentId == commentId)
                .FirstOrDefault();
        }

        public void AddComment(int postId, Comment newComment)
        {
            var post = GetPost(postId);

            post.Comments.Add(newComment);
        }

        public void UpdateComment(Comment updatedComment)
        {
            // no code
        }

        public void DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
        }
        #endregion

        //--------- save ----------//
        public bool Save()
        {
            bool result;
            try
            {
                result = _context.SaveChanges() >= 0;
            }
            catch (DbUpdateException e)
            {
                const int SqlServerViolationOfUniqueIndex = 2601;
                const int SqlServerViolationOfUniqueConstraint = 2627;

                if (e?.InnerException is SqlException sqlEx)
                {
                    if (sqlEx.Number == SqlServerViolationOfUniqueIndex ||
                        sqlEx.Number == SqlServerViolationOfUniqueConstraint)
                    {
                        throw new ApplicationException("Cannot have duplicates.", sqlEx);
                    }

                    // revert entity states
                    foreach (var item in e.Entries)
                    {
                        item.State = EntityState.Detached;
                    }
                }
                // couldn't handle the exception
                throw;
            }
            return result;
        }
    }
}
