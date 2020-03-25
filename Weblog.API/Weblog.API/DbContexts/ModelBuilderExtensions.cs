using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Weblog.API.Entities;

namespace Weblog.API.DbContexts
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var users = new List<User>
            {
                new User
                {
                    UserId = 1,
                    FirstName = "Marcel",
                    LastName = "Wilkerson",
                    EmailAddress = "marcel@blog.net",
                    Password = "password"
                },
                new User
                {
                    UserId = 2,
                    FirstName = "Alaya",
                    LastName = "Perkins",
                    EmailAddress = "alaya@blog.net",
                    Password = "password"
                },
                new User
                {
                    UserId = 3,
                    FirstName = "Fritz",
                    LastName = "Shoaib",
                    EmailAddress = "fritz@blog.net",
                    Password = "password"
                },
                new User
                {
                    UserId = 4,
                    FirstName = "Aston",
                    LastName = "Rivers",
                    EmailAddress = "aston@blog.net",
                    Password = "password"
                },
                new User
                {
                    UserId = 5,
                    FirstName = "Zachery",
                    LastName = "Lucas",
                    EmailAddress = "zachery@blog.net",
                    Password = "password"
                }
            };
            modelBuilder.Entity<User>().HasData(users);

            var blogs = new List<Blog>
            {
                new Blog
                {
                    UserId = 1,
                    BlogId = 1,
                    Title = "Clear And Unbiased Facts About Databases (Without All the Hype)",
                    Excerpt = "My blog about databases"
                },
                new Blog
                {
                    UserId = 1,
                    BlogId = 2,
                    Title = "blog2",
                    Excerpt = "excerpt"
                },
                new Blog
                {
                    UserId = 1,
                    BlogId = 3,
                    Title = "blog3",
                    Excerpt = ""
                },
                new Blog
                {
                    UserId = 1,
                    BlogId = 4,
                    Title = "blog4",
                    Excerpt = "excerpt"
                },
                new Blog
                {
                    UserId = 2,
                    BlogId = 5,
                    Title = "blog5",
                    Excerpt = "excerpt"
                },
                new Blog
                {
                    UserId = 2,
                    BlogId = 6,
                    Title = "blog6",
                    Excerpt = "excerpt"
                },
                new Blog
                {
                    UserId = 5,
                    BlogId = 7,
                    Title = "blog7",
                    Excerpt = ""
                },
                new Blog
                {
                    UserId = 1,
                    BlogId = 8,
                    Title = "blog8",
                    Excerpt = "excerpt"
                },
                new Blog
                {
                    UserId = 1,
                    BlogId = 9,
                    Title = "blog9",
                    Excerpt = "excerpt"
                },
                new Blog
                {
                    UserId = 1,
                    BlogId = 10,
                    Title = "blog10",
                    Excerpt = ""
                },
                new Blog
                {
                    UserId = 3,
                    BlogId = 11,
                    Title = "blog11",
                    Excerpt = "excerpt"
                },
                new Blog
                {
                    UserId = 3,
                    BlogId = 12,
                    Title = "blog12",
                    Excerpt = "excerpt"
                },
                new Blog
                {
                    UserId = 2,
                    BlogId = 13,
                    Title = "blog13",
                    Excerpt = ""
                },
                new Blog
                {
                    UserId = 5,
                    BlogId = 14,
                    Title = "blog14",
                    Excerpt = ""
                },
                new Blog
                {
                    UserId = 1,
                    BlogId = 15,
                    Title = "blog15",
                    Excerpt = "excerpt"
                }
            };
            modelBuilder.Entity<Blog>().HasData(blogs);

            var posts = new List<Post>
            {
                new Post
                {
                    BlogId = 1,
                    PostId = 1,
                    Title = "Why Most Database Fail",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 2,
                    Title = "This Study Will Perfect Your Database: Read Or Miss Out",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 3,
                    Title = "Old School Database",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 4,
                    Title = "17 Tricks About Database You Wish You Knew Before",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 5,
                    Title = "Proof That Database Really Works",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 6,
                    Title = "10 Ways To Reinvent Your Database",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 7,
                    Title = "How To Use Database To Desire",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 8,
                    Title = "If You Do Not (Do)Database Now, You Will Hate Yourself Later",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 9,
                    Title = "9 Ways Database Can Make You Invincible",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 10,
                    Title = "Listen To Your Customers. They Will Tell You All About Database",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 11,
                    Title = "The Secrets To Finding World Class Tools For Your Database Quickly",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 12,
                    Title = "In 10 Minutes, I'll Give You The Truth About Database",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 13,
                    Title = "Little Known Ways to Database",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 14,
                    Title = "5 Sexy Ways To Improve Your Database",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 15,
                    Title = "Best Make Database You Will Read This Year (in 2015)",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 16,
                    Title = "Using 7 Database Strategies Like The Pros",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 17,
                    Title = "15 Tips For Database Success",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 18,
                    Title = "I Don't Want To Spend This Much Time On Database. How About You?",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 19,
                    Title = "How To Make More Database By Doing Less",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 20,
                    Title = "Database Expert Interview",
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                }
            };
            modelBuilder.Entity<Post>().HasData(posts);

            var comments = new List<Comment>
            {
                new Comment
                {
                    PostId = 1,
                    UserId = 1,
                    CommentId = 1,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 3,
                    CommentId = 2,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 5,
                    CommentId = 3,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 1,
                    CommentId = 4,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 1,
                    CommentId = 5,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 1,
                    CommentId = 6,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 2,
                    CommentId = 7,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 3,
                    CommentId = 8,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 3,
                    CommentId = 9,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 4,
                    CommentId = 10,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 4,
                    CommentId = 11,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 1,
                    CommentId = 12,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 4,
                    CommentId = 13,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 2,
                    CommentId = 14,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 2,
                    CommentId = 15,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 3,
                    CommentId = 16,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 4,
                    CommentId = 17,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 2,
                    CommentId = 18,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 2,
                    CommentId = 19,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                },
                new Comment
                {
                    PostId = 1,
                    UserId = 1,
                    CommentId = 20,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
                }
            };
            modelBuilder.Entity<Comment>().HasData(comments);
        }
    }
}
