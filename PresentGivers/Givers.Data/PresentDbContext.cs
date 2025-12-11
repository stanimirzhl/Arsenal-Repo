using Givers.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Givers.Data
{
	public class PresentDbContext : DbContext
	{
		public PresentDbContext(DbContextOptions<PresentDbContext> options) : base (options)
		{
			
		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Gift> Gifts { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Category>()
				.HasMany(x => x.Gifts)
				.WithOne(x => x.Category)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
