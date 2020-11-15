using FluentBlog.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FluentBlog.Infrastructure
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options)
        {
        }

        // 分类和标签
        public DbSet<Meta> Metas { get; set; }
        // 文章
        public DbSet<Archive> Archives { get; set; }
        // 文章和分类的索引
        public DbSet<Relationship> Relationships { get; set; }
        // 文件
        public DbSet<File> Files { get; set; }
        // 设置
        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
        }
    }
}
