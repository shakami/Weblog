using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weblog.API.DbContexts;
using Weblog.API.Entities;

namespace Weblog.API.Services
{
    public class WeblogDataRepository : IWeblogDataRepository
    {
        private readonly WeblogContext _context;

        public WeblogDataRepository(WeblogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddBlog(Blog newBlog)
        {
            var user = GetUser(newBlog.UserId, includeBlogs: true);

            user.Blogs.Add(newBlog);
        }

        public void AddComment(int postId, Comment newComment)
        {
            var post = GetPost(postId, includeComments: true);

            post.Comments.Add(newComment);
        }

        public void AddPost(int blogId, Post newPost)
        {
            var blog = GetBlog(blogId, includePosts: true);

            blog.Posts.Add(newPost);
        }

        public void AddUser(User newUser)
        {
            _context.Users.Add(newUser);
        }

        public bool BlogExists(int blogId)
        {
            return _context.Blogs
                .Any(b => b.BlogId == blogId);
        }

        public void DeleteBlog(Blog blog)
        {
            _context.Blogs.Remove(blog);
        }

        public void DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
        }

        public void DeletePost(Post post)
        {
            _context.Posts.Remove(post);
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }

        public Blog GetBlog(int blogId, bool includePosts)
        {
            if (includePosts)
            {
                var blog = _context.Blogs
                    .Include(b => b.Posts)
                    .Where(b => b.BlogId == blogId)
                    .FirstOrDefault();

                blog.Posts = blog.Posts.OrderByDescending(p => p.TimeCreated).ToList();

                return blog;
            }
            return _context.Blogs
                .Where(b => b.BlogId == blogId)
                .FirstOrDefault();
        }

        public IEnumerable<Blog> GetBlogs()
        {
            return _context.Blogs
                .ToList();
        }

        public IEnumerable<Blog> GetBlogs(int userId)
        {
            var user = GetUser(userId, includeBlogs: true);

            return user.Blogs
                .ToList();
        }

        public Comment GetComment(int commentId)
        {
            return _context.Comments
                .Where(c => c.CommentId == commentId)
                .FirstOrDefault();
        }

        public IEnumerable<Comment> GetComments(int postId)
        {
            var post = GetPost(postId, includeComments: true);

            return post.Comments
                .OrderByDescending(c => c.TimeCreated)
                .ToList();
        }

        public Post GetPost(int postId, bool includeComments)
        {
            if (includeComments)
            {
                var post = _context.Posts
                    .Include(p => p.Comments)
                    .Where(p => p.PostId == postId)
                    .FirstOrDefault();

                post.Comments = post.Comments.OrderByDescending(c => c.TimeCreated).ToList();

                return post;
            }

            return _context.Posts
                .Where(p => p.PostId == postId)
                .FirstOrDefault();
        }

        public IEnumerable<Post> GetPosts(int blogId)
        {
            var blog = GetBlog(blogId, includePosts: true);

            return blog.Posts
                .OrderByDescending(p => p.TimeCreated)
                .ToList();
        }

        public User GetUser(int userId, bool includeBlogs)
        {
            if (includeBlogs)
            {
                return _context.Users
                    .Include(u => u.Blogs)
                    .Where(u => u.UserId == userId)
                    .FirstOrDefault();
            }

            return _context.Users
                .Where(u => u.UserId == userId)
                .FirstOrDefault();
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToList();
        }

        public bool PostExists(int postId)
        {
            return _context.Posts
                .Any(p => p.PostId == postId);
        }

        public bool Save()
        {
            try
            {
                _context.SaveChanges();//  >= 0;
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
            return true;
        }

        public void UpdateBlog(Blog updatedBlog)
        {
            // no code
        }

        public void UpdateComment(Comment updatedComment)
        {
            // no code
        }

        public void UpdatePost(Post updatedPost)
        {
            // no code
        }

        public void UpdateUser(User updatedUser)
        {
            // no code
        }

        public bool UserExists(int userId)
        {
            return _context.Users
                .Any(u => u.UserId == userId);
        }
    }
}
