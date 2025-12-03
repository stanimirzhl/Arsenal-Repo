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
    public class OrganizerController : Controller
    {
        private readonly PartyOrganizationDbContext _context;

        public OrganizerController(PartyOrganizationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orgs = await _context.Organizers.ToListAsync();


			return View(orgs.Select(x => new OrganizerViewModel
            {
                Id = x.Id,
                Name = x.Name,
                PhoneNumber = x.PhoneNumber,
            }).ToList());
        }

        public IActionResult Create()
        {
            return View(new OrganizerViewModel { });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrganizerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var org = new Organizer
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                };
                _context.Add(org);
                await _context.SaveChangesAsync();
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

            var organizer = await _context.Organizers.FindAsync(id);
            if (organizer == null)
            {
                return NotFound();
            }
            return View(new OrganizerViewModel
            {
                Id = organizer.Id,
                Name = organizer.Name,
                PhoneNumber = organizer.PhoneNumber,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, OrganizerViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var org = await _context.Organizers.FindAsync(id);

                    if(org is null)
                    {
                        return NotFound();
                    }

                    org.Name = model.Name;
                    org.PhoneNumber = model.PhoneNumber;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizerExists(model.Id))
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
            var organizer = await _context.Organizers.FindAsync(id);
            if (organizer != null)
            {
                _context.Organizers.Remove(organizer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizerExists(Guid id)
        {
            return _context.Organizers.Any(e => e.Id == id);
        }
    }
}
