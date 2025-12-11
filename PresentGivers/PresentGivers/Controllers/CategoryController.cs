using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Givers.Data;
using Givers.Data.Models;
using PresentGivers.Models;

namespace PresentGivers.Controllers
{
    public class CategoryController : Controller
    {
        private readonly PresentDbContext context;

        public CategoryController(PresentDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var models = await context.Categories.ToListAsync();

			return View(models.Select(x => new CategoryViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Name = model.Name
                };

                context.Add(category);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(new CategoryViewModel
            {
                Name = category.Name
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
					var category = await context.Categories.FindAsync(id);
					if (category == null)
					{
						return NotFound();
					}

                    category.Name = model.Name;

					await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(model.Id))
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
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category != null)
            {
                context.Categories.Remove(category);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return context.Categories.Any(e => e.Id == id);
        }
    }
}
