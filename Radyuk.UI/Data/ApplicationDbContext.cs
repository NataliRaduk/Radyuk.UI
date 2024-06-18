using BAG.DOMAIN.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Radyuk.UI.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
        public DbSet<Bagi> Bagi { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{

		}
	}
}
