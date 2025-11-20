using System.ComponentModel.DataAnnotations;

namespace EmployeeDepartmentProject.Data.Models
{
	public class Department
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }

		public string Code { get; set; }
	}
}
