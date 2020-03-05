using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weblog.API.Entities;

namespace Weblog.API.DbContexts
{
    public class WeblogContext : DbContext
    {
        public WeblogContext(DbContextOptions<WeblogContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasIndex(c => c.PostId)
                .HasName("IX_Comments_PostId");

            modelBuilder.Entity<Comment>()
                .Property(c => c.TimeCreated)
                .HasDefaultValueSql("getDate()");

            modelBuilder.Entity<Post>()
                .Property(p => p.TimeCreated)
                .HasDefaultValueSql("getDate()");

            modelBuilder.Entity<User>()
                .HasAlternateKey(u => u.EmailAddress);

            base.OnModelCreating(modelBuilder);
        }

    }
}
