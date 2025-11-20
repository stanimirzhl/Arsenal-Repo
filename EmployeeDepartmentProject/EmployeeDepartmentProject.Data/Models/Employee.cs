using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDepartmentProject.Data.Models
{
	public class Employee
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public int Salary { get; set; }

		public DateTime DateOfBirth { get; set; }

		[ForeignKey(nameof(Department))]
		[Required]
		public int DepartmentId { get; set; }

		public Department Department { get; set; }
	}
}
