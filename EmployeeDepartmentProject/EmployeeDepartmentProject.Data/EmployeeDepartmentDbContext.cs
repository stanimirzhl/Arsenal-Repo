using EmployeeDepartmentProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDepartmentProject.Data
{
	public class EmployeeDepartmentDbContext : DbContext
	{
		public EmployeeDepartmentDbContext(DbContextOptions<EmployeeDepartmentDbContext> options) : base(options)
		{

		}

		public DbSet<Department> Departments { get; set; } = null!;
		public DbSet<Employee> Employees { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
