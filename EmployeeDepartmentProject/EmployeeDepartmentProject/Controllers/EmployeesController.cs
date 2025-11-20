using EmployeeDepartmentProject.Data;
using EmployeeDepartmentProject.Data.Models;
using EmployeeDepartmentProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDepartmentProject.Controllers
{
	public class EmployeesController : Controller
	{
		private readonly EmployeeDepartmentDbContext _context;

		public EmployeesController(EmployeeDepartmentDbContext context)
		{
			_context = context;
		}

		// GET: Employees
		public async Task<IActionResult> Index()
		{
			return View(await _context.Employees.Include(x => x.Department).ToListAsync());
		}

		// GET: Employees/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var employee = await _context.Employees
				.FirstOrDefaultAsync(m => m.Id == id);
			if (employee == null)
			{
				return NotFound();
			}

			return View(employee);
		}

		// GET: Employees/Create
		public IActionResult Create()
		{
			var employee = new EmployeeViewModel
			{
				Departments = _context.Departments
						.Select(c => new SelectListItem
						{
							Value = c.Id.ToString(),
							Text = c.Name
						})
					.ToList()
			};

			return View(employee);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(EmployeeViewModel employee)
		{
			if (ModelState.IsValid)
			{
				DateTime.TryParseExact(employee.DateOfBirth, "yyyy-MM-dd",
						   CultureInfo.InvariantCulture,
						   DateTimeStyles.None,
						   out var date);

				_context.Add(new Employee
				{
					Name = employee.Name,
					Email = employee.Email,
					DateOfBirth = date,
					Salary = employee.Salary,
					DepartmentId = employee.ChosenId
				});
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(employee);
		}

		// GET: Employees/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var employee = await _context.Employees.FindAsync(id);
			if (employee == null)
			{
				return NotFound();
			}
			return View(new EmployeeViewModel
			{
				Id = employee.Id,
				Email = employee.Email,
				Name = employee.Name,
				Salary = employee.Salary,
				DateOfBirth = employee.DateOfBirth.ToString("yyyy-MM-dd"),
				ChosenId = employee.DepartmentId,
				Departments = _context.Departments
						.Select(c => new SelectListItem
						{
							Value = c.Id.ToString(),
							Text = c.Name
						})
					.ToList()
			});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, EmployeeViewModel employee)
		{
			if (id != employee.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					var employee2 = _context.Employees.Find(employee.Id);

					if(employee2 is null)
					{
						return NotFound();
					}

					DateTime.TryParseExact(employee.DateOfBirth, "yyyy-MM-dd",
						   CultureInfo.InvariantCulture,
						   DateTimeStyles.None,
						   out var date);

					employee2.Name = employee.Name;
					employee2.Salary = employee2.Salary;
					employee2.DateOfBirth = date;
					employee.Email = employee2.Email;
					employee2.DepartmentId = employee2.Id;

					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!EmployeeExists(employee.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(employee);
		}

		// GET: Employees/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var employee = await _context.Employees
				.FirstOrDefaultAsync(m => m.Id == id);
			if (employee == null)
			{
				return NotFound();
			}

			return View(employee);
		}

		// POST: Employees/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var employee = await _context.Employees.FindAsync(id);
			if (employee != null)
			{
				_context.Employees.Remove(employee);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool EmployeeExists(int id)
		{
			return _context.Employees.Any(e => e.Id == id);
		}
	}
}
