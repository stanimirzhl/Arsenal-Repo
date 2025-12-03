using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PartyOrganization.Data;
using PartyOrganization.Data.Models;
using PartyOrganization.Models;

namespace PartyOrganization.Controllers
{
    public class LocationController : Controller
    {
        private readonly PartyOrganizationDbContext context;

        public LocationController(PartyOrganizationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var loc = await context.Locations.ToListAsync();

            return View(loc.Select(x => new LocationViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
            }).ToList());
        }

        public IActionResult Create()
        {
            return View(new LocationViewModel
            {
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LocationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loc = new Location
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Address = model.Address,
				};
                context.Add(loc);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            return View(new LocationViewModel
            {
                Id = location.Id,
                Name = location.Name,
                Address = location.Address,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, LocationViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var loc = await context.Locations.FindAsync(id);
                    if (loc == null)
                        return NotFound();

                    loc.Name = model.Name;
                    loc.Address = model.Address;

                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(model.Id))
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
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var location = await context.Locations.FindAsync(id);
            if (location != null)
            {
                context.Locations.Remove(location);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(Guid id)
        {
            return context.Locations.Any(e => e.Id == id);
        }
    }
}
