using Microsoft.EntityFrameworkCore;
using PartyOrganization.Data.Models;

namespace PartyOrganization.Data
{
	public class PartyOrganizationDbContext : DbContext
	{
		public PartyOrganizationDbContext(DbContextOptions<PartyOrganizationDbContext> options) : base(options)
		{
		}

		public DbSet<Location> Locations { get; set; }

		public DbSet<Organizer> Organizers { get; set; }

		public DbSet<Party> Parties { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Location>()
				.HasMany(x => x.Parties)
				.WithOne(x => x.Location)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Organizer>()
				.HasMany(x => x.Parties)
				.WithOne(x => x.Organizer)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
