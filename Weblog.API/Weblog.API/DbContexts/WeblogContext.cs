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
            Database.EnsureCreated();
        }

        public WeblogContext()
        { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.EmailAddress)
                .HasName("IX_Users_EmailAddress")
                .IsUnique(true)
                .IsClustered(false);

            modelBuilder.Entity<Post>()
                .Property(p => p.TimeCreated)
                .HasDefaultValueSql("getDate()");

            modelBuilder.Entity<Comment>(buildAction =>
            {
                buildAction.HasIndex(c => c.PostId)
                           .HasName("IX_Comments_PostId");

                buildAction.Property(c => c.TimeCreated)
                           .HasDefaultValueSql("getDate()");

                buildAction.HasOne(c => c.User)
                           .WithMany()
                           .HasForeignKey(c => c.UserId)
                           .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
