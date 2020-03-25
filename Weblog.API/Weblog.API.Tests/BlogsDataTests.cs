using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Weblog.API.DbContexts;
using Weblog.API.Entities;
using Weblog.API.ResourceParameters;
using Weblog.API.Services;

namespace Weblog.API.Tests
{
    [TestClass]
    public class BlogsDataTests
    {
        private static SqliteConnection _connection;
        private static WeblogContext _context;
        private static IWeblogDataRepository _repository;
        private static BlogsResourceParameters _resourceParameters;

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

            _resourceParameters = new BlogsResourceParameters
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
        }

        [TestMethod]
        public void AddBlog()
        {
            //-- arrange
            var countBeforeAdd = _repository.GetBlogs(_resourceParameters).Count();

            var blog = new Blog
            {
                Title = "title",
                Excerpt = "excerpt"
            };

            //-- act
            _repository.AddBlog(1, blog);
            _repository.Save();

            var actual = _repository.GetBlogs(_resourceParameters).Count();

            //-- assert
            Assert.AreEqual(countBeforeAdd + 1, actual);

            //-- clean up
            _repository.DeleteBlog(blog);
            _repository.Save();
        }

        [TestMethod]
        public void GetBlogs()
        {
            //-- arrange
            var countBeforeAdd = _repository.GetBlogs(_resourceParameters).Count();

            var blogs = new List<Blog>
            {
                new Blog
                {
                    Title = "title1",
                    Excerpt = "excerpt1"
                },
                new Blog
                {
                    Title = "title2",
                    Excerpt = "excerpt2"
                },
                new Blog
                {
                    Title = "title3",
                    Excerpt = "excerpt3"
                },
                new Blog
                {
                    Title = "title4",
                    Excerpt = "excerpt4"
                }
            };

            foreach (var blog in blogs)
            {
                _repository.AddBlog(1, blog);
            }
            _repository.Save();

            //-- act
            var actual = _repository.GetBlogs(_resourceParameters);

            //-- assert
            Assert.AreEqual("title1", actual.First().Title);
            Assert.AreEqual(countBeforeAdd + 4, actual.Count());

            //-- cleanup
            foreach (var blog in blogs)
            {
                _repository.DeleteBlog(blog);
            }
            _repository.Save();
        }

        [TestMethod]
        public void GetBlog()
        {
            //-- arrange
            var blog = new Blog
            {
                Title = "title",
                Excerpt = "excerpt"
            };

            _repository.AddBlog(1, blog);
            _repository.Save();

            //-- act
            var actual = _repository.GetBlog(1);

            //-- assert
            Assert.AreEqual("title", actual.Title);

            //-- cleanup
            _repository.DeleteBlog(blog);
            _repository.Save();
        }

        [TestMethod]
        public void UpdateBlog()
        {
            //-- arrange
            var blog = new Blog
            {
                Title = "old-title",
                Excerpt = "old-excerpt"
            };

            _repository.AddBlog(1, blog);
            _repository.Save();

            blog.Title = "new-title";
            blog.Excerpt = "new-excerpt";

            //-- act
            _repository.UpdateBlog(blog);
            _repository.Save();

            var actual = _repository.GetBlog(1);

            //-- assert
            Assert.AreEqual("new-title", actual.Title);
            Assert.AreEqual("new-excerpt", actual.Excerpt);

            //-- cleanup
            _repository.DeleteBlog(blog);
            _repository.Save();
        }

        [TestMethod]
        public void DeleteBlog()
        {
            //-- arrange
            var blog = new Blog
            {
                Title = "title",
                Excerpt = "excerpt"
            };

            _repository.AddBlog(1, blog);
            _repository.Save();

            var countBeforeDelete = _repository.GetBlogs(1, _resourceParameters).Count();

            //-- act
            _repository.DeleteBlog(blog);
            _repository.Save();

            //-- assert
            var actual = _repository.GetBlogs(1, _resourceParameters).Count();

            Assert.AreEqual(countBeforeDelete - 1, actual);
        }

        [TestMethod]
        public void BlogExists()
        {
            //-- arrange
            var blog = new Blog
            {
                Title = "title",
                Excerpt = "excerpt"
            };

            _repository.AddBlog(1, blog);
            _repository.Save();

            //-- act
            var actual = _repository.BlogExists(1);

            //-- assert
            Assert.IsTrue(actual);

            //-- clean up
            _repository.DeleteBlog(blog);
            _repository.Save();
        }

        [TestMethod]
        public void BlogExistsInvalidID()
        {
            //-- arrange

            //-- act
            var actual = _repository.BlogExists(1);

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
