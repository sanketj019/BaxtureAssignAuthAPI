using BaxtureAssignAuthAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BaxtureAssignAuthAPI.DBContext
{
    public class UserDbContext:DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
           : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; } = null!;

       

    }
}
