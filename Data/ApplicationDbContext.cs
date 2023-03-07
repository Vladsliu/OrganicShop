
using Microsoft.EntityFrameworkCore;
using OrganicShop2.Models.Data;

namespace CulinaryClub.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
		{

		}
		public DbSet<PagesDTO> MasterClasses { get; set; }
    }
}
