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
    public class CommentsDataTests
    {
        private static SqliteConnection _connection;
        private static WeblogContext _context;
        private static IWeblogDataRepository _repository;
        private static CommentsResourceParameters _resourceParameters;

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

            _resourceParameters = new CommentsResourceParameters
            {
                PageNumber = 1,
                PageSize = 10
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
                Title = "blog-title",
                Excerpt = "blog-excerpt"
            });
            _repository.Save();

            _repository.AddPost(1, new Post
            {
                Title = "post-title",
                Body = "post-body",
                TimeCreated = DateTime.Now
            });
            _repository.Save();
        }

        [TestMethod]
        public void AddComment()
        {
            //-- arrange
            var countBeforeAdd = _repository.GetComments(1, _resourceParameters).Count();

            var comment = new Comment
            {
                UserId = 1,
                Body = "body",
                TimeCreated = DateTime.Now
            };

            //-- act
            _repository.AddComment(1, comment);
            _repository.Save();

            var actual = _repository.GetComments(1, _resourceParameters).Count();

            //-- assert
            Assert.AreEqual(countBeforeAdd + 1, actual);

            //-- clean up
            _repository.DeleteComment(comment);
            _repository.Save();
        }

        [TestMethod]
        public void GetComments()
        {
            //-- arrange
            var countBeforeAdd = _repository.GetComments(1, _resourceParameters).Count();

            var comments = new List<Comment>
            {
                new Comment
                {
                    UserId = 1,
                    Body = "body1",
                    TimeCreated = DateTime.Now.AddDays(-1)
                },
                new Comment
                {
                    UserId = 1,
                    Body = "body2",
                    TimeCreated = DateTime.Now.AddDays(-2)
                },
                new Comment
                {
                    UserId = 1,
                    Body = "body3",
                    TimeCreated = DateTime.Now.AddDays(-3)
                }
            };

            foreach (var comment in comments)
            {
                _repository.AddComment(1, comment);
            }
            _repository.Save();

            //-- act
            var actual = _repository.GetComments(1, _resourceParameters);

            //-- assert
            Assert.AreEqual("body1", actual.First().Body);
            Assert.AreEqual(countBeforeAdd + 3, actual.Count());

            //-- cleanup
            foreach (var comment in comments)
            {
                _repository.DeleteComment(comment);
            }
            _repository.Save();
        }

        [TestMethod]
        public void GetComment()
        {
            //-- arrange
            var comment = new Comment
            {
                UserId = 1,
                Body = "body",
                TimeCreated = DateTime.Now
            };

            _repository.AddComment(1, comment);
            _repository.Save();

            //-- act
            var actual = _repository.GetComment(1);

            //-- assert
            Assert.AreEqual("body", actual.Body);

            //-- cleanup
            _repository.DeleteComment(comment);
            _repository.Save();
        }

        [TestMethod]
        public void UpdateComment()
        {
            //-- arrange
            var comment = new Comment
            {
                UserId = 1,
                Body = "old-body",
                TimeCreated = DateTime.Now
            };

            _repository.AddComment(1, comment);
            _repository.Save();

            comment.Body = "new-body";

            //-- act
            _repository.UpdateComment(comment);
            _repository.Save();

            var actual = _repository.GetComment(1);

            //-- assert
            Assert.AreEqual("new-body", actual.Body);

            //-- cleanup
            _repository.DeleteComment(comment);
            _repository.Save();
        }

        [TestMethod]
        public void DeleteComment()
        {
            //-- arrange
            var comment = new Comment
            {
                UserId = 1,
                Body = "body",
                TimeCreated = DateTime.Now
            };

            _repository.AddComment(1, comment);
            _repository.Save();

            var countBeforeDelete = _repository.GetComments(1, _resourceParameters).Count();

            //-- act
            _repository.DeleteComment(comment);
            _repository.Save();

            //-- assert
            var actual = _repository.GetComments(1, _resourceParameters).Count();

            Assert.AreEqual(countBeforeDelete - 1, actual);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _connection.Close();
        }
    }
}
