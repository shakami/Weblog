using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Weblog.API.DbContexts;
using Weblog.API.Entities;
using Weblog.API.ResourceParameters;
using Weblog.API.Services;

namespace Weblog.API.Tests
{
    [TestClass]
    public class PostsDataTests
    {
        private static SqliteConnection _connection;
        private static WeblogContext _context;
        private static IWeblogDataRepository _repository;
        private static PostsResourceParameters _resourceParameters;

        [TestInitialize]
        public void TestInitialize()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<WeblogContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new WeblogContext(options);
            _context.Database.EnsureCreated();

            _repository = new WeblogDataRepository(_context);

            _resourceParameters = new PostsResourceParameters
            {
                PageNumber = 1,
                PageSize = 10,
                SearchQuery = ""
            };

            _repository.AddUser(new User
            {
                FirstName = "fname",
                LastName = "lname",
                EmailAddress = "user@email",
                Password = "password"
            });
            _repository.Save();

            _repository.AddBlog(1, new Blog
            {
                Title = "title",
                Excerpt = "excerpt"
            });
            _repository.Save();
        }

        [TestMethod]
        public void AddPost()
        {
            //-- arrange
            var countBeforeAdd = _repository.GetPosts(1, _resourceParameters).Count();

            var post = new Post
            {
                Title = "title",
                Body = "body",
                TimeCreated = DateTime.Now
            };

            //-- act
            _repository.AddPost(1, post);
            _repository.Save();

            var actual = _repository.GetPosts(1, _resourceParameters).Count();

            //-- assert
            Assert.AreEqual(countBeforeAdd + 1, actual);

            //-- clean up
            _repository.DeletePost(post);
            _repository.Save();
        }
        
        [TestMethod]
        public void GetPosts()
        {
            //-- arrange
            var countBeforeAdd = _repository.GetPosts(1, _resourceParameters).Count();

            var posts = new List<Post>
            {
                new Post
                {
                    Title = "title1",
                    Body = "body1",
                    TimeCreated = DateTime.Now.AddDays(-1)
                },
                new Post
                {
                    Title = "title2",
                    Body = "body2",
                    TimeCreated = DateTime.Now.AddDays(-2)
                },
                new Post
                {
                    Title = "title3",
                    Body = "body3",
                    TimeCreated = DateTime.Now.AddDays(-3)
                }
            };

            foreach (var post in posts)
            {
                _repository.AddPost(1, post);
            }
            _repository.Save();

            //-- act
            var actual = _repository.GetPosts(1, _resourceParameters);

            //-- assert
            Assert.AreEqual("title1", actual.First().Title);
            Assert.AreEqual(countBeforeAdd + 3, actual.Count());

            //-- cleanup
            foreach (var post in posts)
            {
                _repository.DeletePost(post);
            }
            _repository.Save();
        }
        
        [TestMethod]
        public void GetPost()
        {
            //-- arrange
            var post = new Post
            {
                Title = "title",
                Body = "body",
                TimeCreated = DateTime.Now
            };

            _repository.AddPost(1, post);
            _repository.Save();

            //-- act
            var actual = _repository.GetPost(1);

            //-- assert
            Assert.AreEqual("title", actual.Title);

            //-- cleanup
            _repository.DeletePost(post);
            _repository.Save();
        }
        
        [TestMethod]
        public void UpdatePost()
        {
            //-- arrange
            var post = new Post
            {
                Title = "old-title",
                Body = "old-body",
                TimeCreated = DateTime.Now
            };

            _repository.AddPost(1, post);
            _repository.Save();

            post.Title = "new-title";
            post.Body = "new-body";

            //-- act
            _repository.UpdatePost(post);
            _repository.Save();

            var actual = _repository.GetPost(1);

            //-- assert
            Assert.AreEqual("new-title", actual.Title);
            Assert.AreEqual("new-body", actual.Body);

            //-- cleanup
            _repository.DeletePost(post);
            _repository.Save();
        }
        
        [TestMethod]
        public void DeletePost()
        {
            //-- arrange
            var post = new Post
            {
                Title = "title",
                Body = "body",
                TimeCreated = DateTime.Now
            };

            _repository.AddPost(1, post);
            _repository.Save();

            var countBeforeDelete = _repository.GetPosts(1, _resourceParameters).Count();

            //-- act
            _repository.DeletePost(post);
            _repository.Save();

            //-- assert
            var actual = _repository.GetPosts(1, _resourceParameters).Count();

            Assert.AreEqual(countBeforeDelete - 1, actual);
        }
        
        [TestMethod]
        public void PostExists()
        {
            //-- arrange
            var post = new Post
            {
                Title = "title",
                Body = "body",
                TimeCreated = DateTime.Now
            };

            _repository.AddPost(1, post);
            _repository.Save();

            //-- act
            var actual = _repository.PostExists(1);

            //-- assert
            Assert.IsTrue(actual);

            //-- clean up
            _repository.DeletePost(post);
            _repository.Save();
        }
        
        [TestMethod]
        public void PostExistsInvalidID()
        {
            //-- arrange

            //-- act
            var actual = _repository.PostExists(1);

            //-- assert
            Assert.IsFalse(actual);
        }
        
        [TestCleanup]
        public void TestCleanup()
        {
            _connection.Close();
        }
    }
}
