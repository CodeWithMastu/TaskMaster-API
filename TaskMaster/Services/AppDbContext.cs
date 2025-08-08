using Microsoft.EntityFrameworkCore;
using TaskMaster.Models;

namespace TaskMaster.Services
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TasksItem> Tasks { get; set; }
    }
}
