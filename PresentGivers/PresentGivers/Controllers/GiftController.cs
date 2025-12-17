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
using Microsoft.AspNetCore.Authorization;

namespace PresentGivers.Controllers
{
    public class GiftController : Controller
    {
        private readonly PresentDbContext context;

        public GiftController(PresentDbContext context)
        {
            this.context = context;
        }

        [Authorize]
		public async Task<IActionResult> Bekommen()
		{
			var gifts = await context.Gifts
				.Include(g => g.Category)
				.Where(g => g.IsTaken == false) 
				.ToListAsync();

			if (!gifts.Any())
			{
				TempData["ErrorMessage"] = "Няма налични подаръци!";
				return RedirectToAction("Index", "Home");
			}

			var rand = new Random();
			int index = rand.Next(gifts.Count);
			var selectedGift = gifts[index];

            var category = await context.Categories.FindAsync(selectedGift.CategoryId);

            selectedGift.IsTaken = true;

            await context.SaveChangesAsync();

			return View(new GiftViewModel
            {
                Id = selectedGift.Id,
                Name = selectedGift.Name,
                Description = selectedGift.Description,
                ImageUrl = selectedGift.ImageUrl,
                CategoryName = category.Name
            });
		}


		public async Task<IActionResult> Index(int? categoryId = null)
        {
            var presentDbContext = await context.Gifts.Include(g => g.Category).ToListAsync();

            ViewBag.CategoryCount = context.Categories.Count();

			ViewBag.Categories = new SelectList(await context.Categories.ToListAsync(), "Id", "Name", categoryId);

			if (categoryId.HasValue)
            {
                presentDbContext = presentDbContext.Where(x => x.CategoryId == categoryId).ToList();

                var category = await context.Categories.FindAsync(categoryId.Value);

                if(category is null)
                {
                    return NotFound();
                }

                TempData["Category"] = $"Подаръци с категория {category.Name}";

               //ViewBag.Categories = new SelectList(await context.Categories.ToListAsync(), "Id", "Name");
            }


            return View(presentDbContext.Select(x => new GiftViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                CategoryName = x.Category.Name,
                IsTaken = x.IsTaken

            }).ToList());
        }

        public IActionResult Create()
        {
            if (!context.Categories.Any())
            {
                return Forbid();
            }

            var model = new GiftViewModel
            {
                Categories = context.Categories.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                }).ToList()
            };           

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GiftViewModel gift)
        {
            if (ModelState.IsValid)
            {
                var gift2 = new Gift
                {
                    Id = gift.Id,
                    Name = gift.Name,
                    Description = gift.Description,
                    ImageUrl = gift.ImageUrl,
                    CategoryId = gift.CategoryId
                };

                context.Add(gift2);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            gift.Categories = context.Categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
            }).ToList();


			return View(gift);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gift = await context.Gifts.FindAsync(id);
            if (gift == null)
            {
                return NotFound();
            }

            var model = new GiftViewModel
            {
                Name = gift.Name,
                Description = gift.Description,
                ImageUrl = gift.ImageUrl,
                CategoryId = gift.CategoryId,               
            };

            model.Categories = context.Categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
            }).ToList();


			return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GiftViewModel gift)
        {
            if (id != gift.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var gift2 = await context.Gifts.FindAsync(id);
                    if (gift2 == null) { return NotFound(); }

                    gift2.Name = gift.Name;
                    gift2.Description = gift.Description;
                    gift2.ImageUrl = gift.ImageUrl;
                    gift2.CategoryId = gift.CategoryId;

                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiftExists(gift.Id))
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
            gift.Categories = context.Categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
            }).ToList();

			return View(gift);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gift = await context.Gifts.FindAsync(id);
            if (gift != null)
            {
                context.Gifts.Remove(gift);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiftExists(int id)
        {
            return context.Gifts.Any(e => e.Id == id);
        }
    }
}
