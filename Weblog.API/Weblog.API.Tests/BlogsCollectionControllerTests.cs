using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Weblog.API.DbContexts;
using Weblog.API.Entities;
using Weblog.API.ResourceParameters;
using Weblog.API.Services;

namespace Weblog.API.Tests
{
    [TestClass]
    public class BlogsCollectionControllerTests
    {
        [ClassInitialize]
        public static void BlogsCollectionController(TestContext context)
        {

        }

        [TestInitialize]
        public void Setup()
        {
            
            

        }

        [TestMethod]
        public void GetAllBlogs()
        {
            IQueryable<Blog> blogs = new List<Blog>
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
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Blog>>();
            mockSet.As<IQueryable<Blog>>().Setup(m => m.Provider).Returns(blogs.Provider);
            mockSet.As<IQueryable<Blog>>().Setup(m => m.Expression).Returns(blogs.Expression);
            mockSet.As<IQueryable<Blog>>().Setup(m => m.ElementType).Returns(blogs.ElementType);
            mockSet.As<IQueryable<Blog>>().Setup(m => m.GetEnumerator()).Returns(blogs.GetEnumerator());

            var mockContext = new Mock<WeblogContext>();
            mockContext.Setup(c => c.Blogs).Returns(mockSet.Object);

            var repo = new WeblogDataRepository(mockContext.Object);
            var resourceParams = new BlogsResourceParameters
            {
                PageNumber = 1,
                PageSize = 10,
                SearchQuery = ""
            };

            var actual = repo.GetBlogs(resourceParams);

            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual("title1", actual.First().Title);

        }
    }
}
