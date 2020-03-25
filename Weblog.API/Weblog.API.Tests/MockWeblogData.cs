using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weblog.API.DbContexts;
using Weblog.API.Entities;

namespace Weblog.API.Tests
{
    public class MockWeblogData
    {
        public WeblogContext Context { get; set; }

        private readonly Mock<WeblogContext> _mockContext
            = new Mock<WeblogContext>();

        private readonly IQueryable<User> _users;
        private readonly IQueryable<Blog> _blogs;
        private readonly IQueryable<Post> _posts;
        private readonly IQueryable<Comment> _comments;

        public MockWeblogData()
        {
            _users = GetUserData();
            _blogs = GetBlogData();
            _posts = GetPostData();
            _comments = GetCommentData();

            Context = InitializeContext();
        }

        private IQueryable<User> GetUserData()
        {
            return new List<User>
            {
                new User
                {
                    UserId = 1,
                    FirstName = "fname1",
                    LastName = "lname1",
                    EmailAddress = "1@users",
                    Password = "secret"
                },
                new User
                {
                    UserId = 2,
                    FirstName = "fname2",
                    LastName = "lname2",
                    EmailAddress = "2@users",
                    Password = "secret"
                },
                new User
                {
                    UserId = 3,
                    FirstName = "fname3",
                    LastName = "lname3",
                    EmailAddress = "3@users",
                    Password = "secret"
                }
            }.AsQueryable();
        }

        private IQueryable<Blog> GetBlogData()
        {
            return new List<Blog>
            {
                new Blog
                {
                    UserId = 1,
                    BlogId = 1,
                    Title = "title1",
                    Excerpt = "excerpt1"
                },
                new Blog
                {
                    UserId = 1,
                    BlogId = 2,
                    Title = "title2",
                    Excerpt = "excerpt2"
                },
                new Blog
                {
                    UserId = 2,
                    BlogId = 3,
                    Title = "title3",
                    Excerpt = "excerpt3"
                }
            }.AsQueryable();
        }

        private IQueryable<Post> GetPostData()
        {
            return new List<Post>
            {
                new Post
                {
                    BlogId = 1,
                    PostId = 1,
                    Title = "post1",
                    Body = "body1",
                    TimeCreated = new DateTime(2020, 3, 1)
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 2,
                    Title = "post2",
                    Body = "body2",
                    TimeCreated = new DateTime(2020, 3, 5)
                },new Post
                {
                    BlogId = 1,
                    PostId = 3,
                    Title = "post3",
                    Body = "body3",
                    TimeCreated = new DateTime(2020, 3, 6)
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 4,
                    Title = "post4",
                    Body = "body4",
                    TimeCreated = new DateTime(2020, 3, 19)
                }
            }.AsQueryable();
        }
        
        private IQueryable<Comment> GetCommentData()
        {
            return new List<Comment>
            {
                new Comment
                {
                    PostId = 1,
                    UserId = 3,
                    CommentId = 1,
                    Body = "comment1",
                    TimeCreated = new DateTime(2020, 3, 1, 10, 30, 0)
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 1,
                    CommentId = 1,
                    Body = "comment2",
                    TimeCreated = new DateTime(2020, 3, 1, 10, 45, 0)
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 2,
                    CommentId = 1,
                    Body = "comment3",
                    TimeCreated = new DateTime(2020, 3, 1, 11, 0, 0)
                }
            }.AsQueryable();
        }

        private WeblogContext InitializeContext()
        {
            var mockUserSet = new Mock<DbSet<User>>();
            mockUserSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(_users.Provider);
            mockUserSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(_users.Expression);
            mockUserSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(_users.ElementType);
            mockUserSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(_users.GetEnumerator());

            var mockBlogSet = new Mock<DbSet<Blog>>();
            mockBlogSet.As<IQueryable<Blog>>().Setup(m => m.Provider).Returns(_blogs.Provider);
            mockBlogSet.As<IQueryable<Blog>>().Setup(m => m.Expression).Returns(_blogs.Expression);
            mockBlogSet.As<IQueryable<Blog>>().Setup(m => m.ElementType).Returns(_blogs.ElementType);
            mockBlogSet.As<IQueryable<Blog>>().Setup(m => m.GetEnumerator()).Returns(_blogs.GetEnumerator());

            var mockPostSet = new Mock<DbSet<Post>>();
            mockPostSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(_posts.Provider);
            mockPostSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(_posts.Expression);
            mockPostSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(_posts.ElementType);
            mockPostSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(_posts.GetEnumerator());

            var mockCommentSet = new Mock<DbSet<Comment>>();
            mockCommentSet.As<IQueryable<Comment>>().Setup(m => m.Provider).Returns(_comments.Provider);
            mockCommentSet.As<IQueryable<Comment>>().Setup(m => m.Expression).Returns(_comments.Expression);
            mockCommentSet.As<IQueryable<Comment>>().Setup(m => m.ElementType).Returns(_comments.ElementType);
            mockCommentSet.As<IQueryable<Comment>>().Setup(m => m.GetEnumerator()).Returns(_comments.GetEnumerator());

            _mockContext.Setup(c => c.Users).Returns(mockUserSet.Object);
            _mockContext.Setup(c => c.Blogs).Returns(mockBlogSet.Object);
            _mockContext.Setup(c => c.Posts).Returns(mockPostSet.Object);
            _mockContext.Setup(c => c.Comments).Returns(mockCommentSet.Object);

            return _mockContext.Object;
        }

    }
}
