using CatsProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsProject.Data
{
	public class CatsProjectDbContext : DbContext
	{
		public CatsProjectDbContext(DbContextOptions<CatsProjectDbContext> options) : base(options)
		{

		}

		public DbSet<Cat> Cats { get; set; }

		public DbSet<Breed> Breeds { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Breed>().HasMany(x => x.Cats).WithOne(x => x.Breed).OnDelete(DeleteBehavior.SetNull);
		}
	}
}
