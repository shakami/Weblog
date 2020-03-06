﻿using Microsoft.EntityFrameworkCore;
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

        public void AddBlog(int userId, Blog newBlog)
        {
            var user = GetUser(userId, includeBlogs: false);

            user.Blogs.Add(newBlog);
        }

        public void AddComment(int postId, Comment newComment)
        {
            var post = GetPost(postId);

            post.Comments.Add(newComment);
        }

        public void AddPost(int blogId, Post newPost)
        {
            var blog = GetBlog(blogId);

            blog.Posts.Add(newPost);
        }

        public void AddUser(User newUser)
        {
            throw new NotImplementedException();
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

        public Blog GetBlog(int blogId)
        {
            return _context.Blogs
                .Where(b => b.BlogId == blogId)
                .FirstOrDefault();
        }

        public IEnumerable<Blog> GetBlogs(int userId)
        {
            return _context.Blogs
                .ToList();
        }

        public IEnumerable<Comment> GetComments(int postId)
        {
            var post = GetPost(postId);

            return post.Comments
                .OrderByDescending(c => c.TimeCreated)
                .ToList();
        }

        public Post GetPost(int postId)
        {
            return _context.Posts
                .Where(p => p.PostId == postId)
                .FirstOrDefault();
        }

        public IEnumerable<Post> GetPosts(int blogId)
        {
            var blog = GetBlog(blogId);

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

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
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
            // no code
        }
    }
}