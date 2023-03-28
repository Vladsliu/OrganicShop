
using Microsoft.EntityFrameworkCore;
using OrganicShop2.Models.Data;

namespace CulinaryClub.Data
{
	public class Db : DbContext
	{
       

        public Db(DbContextOptions<Db> options) : base(options)
        {

        }
        public DbSet<PagesDTO> Pages { get; set; }
        public DbSet<SidebarDTO> Sidebars { get; set; }
        public DbSet<CategoryDTO> Categories { get; set; }
    }
}
