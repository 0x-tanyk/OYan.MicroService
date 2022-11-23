using Microsoft.EntityFrameworkCore;
using OYan.EF.MySQL.Models;

namespace OYan.EF.MySQL.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class OYanDbContext : DbContext
    {
        /// <summary>
        /// 博客
        /// </summary>
        public DbSet<Blog> Blogs { get; set; }

        /// <summary>
        /// 文章
        /// </summary>
        public DbSet<Post> Posts { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="options"></param>
        public OYanDbContext(DbContextOptions<OYanDbContext> options)
            : base(options) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
