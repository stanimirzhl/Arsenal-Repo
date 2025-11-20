using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeDepartmentProject.Models
{
	public class EmployeeViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public int Salary { get; set; }

		public string DateOfBirth { get; set; }

		public List<SelectListItem> Departments { get; set; } = new List<SelectListItem>();

		public int ChosenId { get; set; }
	}
}
