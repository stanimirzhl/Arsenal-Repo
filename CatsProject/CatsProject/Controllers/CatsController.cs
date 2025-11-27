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
    public class CatsController : Controller
    {
        private readonly CatsProjectDbContext context;

        public CatsController(CatsProjectDbContext context)
        {
            this.context = context;
        }

        // GET: Cats
        public async Task<IActionResult> Index()
        {
            var cats = context.Cats.Include(c => c.Breed);

            var vms = cats.Select(x => new CatViewModel
			{
				Id = x.Id,
				Name = x.Name,
				Age = x.Age,
                BreedName = x.Breed.Title,
                BreedId = x.Breed.Id,
                ImageUrl = x.ImageUrl
            }).ToList();

            return View(vms);
        }

        // GET: Cats/Create
        public IActionResult Create()
        {
            var model = new CatViewModel
            {
                Breeds = context.Breeds.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Title
                }).ToList()
            };


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CatViewModel model)
        {
            if (ModelState.IsValid)
            {
                context.Add(new Cat
                {
                    Name = model.Name,
                    Age = model.Age,
                    BreedId = model.BreedId,
                    ImageUrl = model.ImageUrl,
                    Id = new Guid()
                });
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            model.Breeds = context.Breeds.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Title
            }).ToList();

            return View(model);
        }

        // GET: Cats/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cat = await context.Cats.FindAsync(id);
            if (cat == null)
            {
                return NotFound();
            }

            var model = new CatViewModel
            {
                Id = cat.Id,
                Name = cat.Name,
                Age = cat.Age,
                ImageUrl= cat.ImageUrl,
                BreedId = cat.BreedId,
            };

			model.Breeds = context.Breeds.Select(x => new SelectListItem
			{
				Value = x.Id.ToString(),
				Text = x.Title
			}).ToList();

			return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CatViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cat = context.Cats.Find(id);

                    cat.ImageUrl = model.ImageUrl;
                    cat.Name = model.Name;
                    cat.Age = model.Age;
                    cat.BreedId = model.BreedId;

                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatExists(model.Id))
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

			model.Breeds = context.Breeds.Select(x => new SelectListItem
			{
				Value = x.Id.ToString(),
				Text = x.Title
			}).ToList();

			return View(model);
        }

        // POST: Cats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cat = await context.Cats.FindAsync(id);
            if (cat != null)
            {
                context.Cats.Remove(cat);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatExists(Guid id)
        {
            return context.Cats.Any(e => e.Id == id);
        }
    }
}
