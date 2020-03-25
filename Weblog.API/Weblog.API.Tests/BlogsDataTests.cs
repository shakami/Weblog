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
    public class BlogsDataTests
    {
        private static readonly WeblogContext _context
            = new MockWeblogData().Context;

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
            var repo = new WeblogDataRepository(_context);
            var resourceParams = new BlogsResourceParameters
            {
                PageNumber = 1,
                PageSize = 10
            };

            var actual = repo.GetBlogs(resourceParams);

            Assert.AreEqual(3, actual.Count());
            Assert.AreEqual("title1", actual.First().Title);

        }
    }
}
