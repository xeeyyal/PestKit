using Microsoft.EntityFrameworkCore;
using PestKitAB104.Models;

namespace PestKitAB104.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<BlogTag> BlogsTags { get; set; }
        public DbSet<Tag> Tags { get; set; }

    }
}
