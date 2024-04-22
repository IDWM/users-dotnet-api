using Microsoft.EntityFrameworkCore;

namespace AYUD_MINIMAL_API
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();

    }
}