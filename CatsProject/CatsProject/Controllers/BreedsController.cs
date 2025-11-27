using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CatsProject.Data;
using CatsProject.Data.Models;
using CatsProject.Models;

namespace CatsProject.Controllers
{
    public class BreedsController : Controller
    {
        private readonly CatsProjectDbContext context;

        public BreedsController(CatsProjectDbContext context)
        {
            this.context = context;
        }

        // GET: Breeds
        public async Task<IActionResult> Index()
        {
            var breeds = await context.Breeds.ToListAsync();


			return View(breeds.Select(x => new BreedViewModel
            {
                Id = x.Id,
                Title = x.Title,
            }).ToList());
        }

        // GET: Breeds/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BreedViewModel model)
        {
            if (ModelState.IsValid)
            {
                var breed = new Breed
                {
                    Id = new Guid(),
                    Title = model.Title,
                };

                await context.AddAsync(breed);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Breeds/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var breed = await context.Breeds.FindAsync(id);
            if (breed == null)
            {
                return NotFound();
            }
            return View(new BreedViewModel
            {
                Id = breed.Id,
                Title = breed.Title,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, BreedViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var breed = await context.Breeds.FindAsync(model.Id);

                    breed.Title = model.Title;

                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BreedExists(model.Id))
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

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var breed = await context.Breeds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (breed == null)
            {
                return NotFound();
            }

            return View(breed);
        }

        // POST: Breeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var breed = await context.Breeds.FindAsync(id);
            if (breed != null)
            {
                context.Breeds.Remove(breed);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BreedExists(Guid id)
        {
            return context.Breeds.Any(e => e.Id == id);
        }
    }
}
