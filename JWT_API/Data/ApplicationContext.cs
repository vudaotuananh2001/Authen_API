using JWT_API.Models;
using Microsoft.EntityFrameworkCore;

namespace JWT_API.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }  

    }
}
