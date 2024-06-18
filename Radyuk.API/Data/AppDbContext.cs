using BAG.DOMAIN.Entities;
using Microsoft.EntityFrameworkCore;

namespace Radyuk.API.Data
{
    public class AppDbContext : DbContext
    {


        public DbSet<Bagi> Bagi { get; set; }
        public DbSet<Category> Categories { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    }
}
