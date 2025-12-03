using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PartyOrganization.Data;
using PartyOrganization.Data.Models;
using PartyOrganization.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PartyOrganization.Controllers
{
	public class PartyController : Controller
	{
		private readonly PartyOrganizationDbContext _context;

		public PartyController(PartyOrganizationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var partyOrganizationDbContext = _context.Parties.Include(p => p.Location).Include(p => p.Organizer);
			return View(partyOrganizationDbContext.Select(x => new PartyViewModel
			{
				Id = x.Id,
				Title = x.Title,
				Description = x.Description,
				Date = x.Date.ToString("MM-dd-yyyy'г.' HH:mm:ss"),
				Image = x.ImageUrl,
				LocationName = x.Location.Name,
				OrganizerName = x.Organizer.Name
			}).ToList());
		}

		public IActionResult Create()
		{
			return View(new PartyViewModel
			{
				Locations = _context.Locations.Select(x => new SelectListItem
				{
					Text = x.Name,
					Value = x.Id.ToString()
				}).ToList(),

				Organizers = _context.Organizers.Select(x => new SelectListItem
				{
					Text = x.Name,
					Value = x.Id.ToString()
				}).ToList()
			});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(PartyViewModel party)
		{
			if (ModelState.IsValid)
			{
				DateTime.TryParseExact(party.Date, "yyyy-MM-ddTHH:mm:ss",
						CultureInfo.InvariantCulture,
						DateTimeStyles.None,
						out DateTime date);

				var party2 = new Party
				{
					Title = party.Title,
					Description = party.Description,
					ImageUrl = party.Image,
					LocationId = party.LocationId,
					OrganizerId = party.OrganizerId,
					Date = date
				};

				_context.Add(party2);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			party.Locations = _context.Locations.Select(x => new SelectListItem
			{
				Text = x.Name,
				Value = x.Id.ToString()
			}).ToList();

			party.Organizers = _context.Organizers.Select(x => new SelectListItem
			{
				Text = x.Name,
				Value = x.Id.ToString()
			}).ToList();


			return View(party);
		}

		// GET: Party/Edit/5
		public async Task<IActionResult> Edit(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var model = await _context.Parties.FindAsync(id);
			if (model == null)
			{
				return NotFound();
			}

			var party = new PartyViewModel
			{
				Id = model.Id,
				Title = model.Title,
				Date = model.Date.ToString(),
				Description = model.Description,
				Image = model.ImageUrl,
				LocationId = model.LocationId,
				OrganizerId = model.OrganizerId,
			};

			party.Locations = _context.Locations.Select(x => new SelectListItem
			{
				Text = x.Name,
				Value = x.Id.ToString()
			}).ToList();

			party.Organizers = _context.Organizers.Select(x => new SelectListItem
			{
				Text = x.Name,
				Value = x.Id.ToString()
			}).ToList();
			return View(party);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, PartyViewModel party)
		{

			if (ModelState.IsValid)
			{
				try
				{
					var prt = await _context.Parties.FindAsync(id);
					if (prt is null)
					{
						return NotFound();
					}

					DateTime.TryParseExact(party.Date,"yyyy-MM-ddTHH:mm:ss",
						CultureInfo.InvariantCulture,
						DateTimeStyles.None,
						out DateTime date);

					prt.Title = party.Title;
					prt.Description = party.Description;
					prt.ImageUrl = party.Image;
					prt.LocationId = party.LocationId;
					prt.OrganizerId = party.OrganizerId;
					prt.Date = date;

					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PartyExists(party.Id))
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
			party.Locations = _context.Locations.Select(x => new SelectListItem
			{
				Text = x.Name,
				Value = x.Id.ToString()
			}).ToList();

			party.Organizers = _context.Organizers.Select(x => new SelectListItem
			{
				Text = x.Name,
				Value = x.Id.ToString()
			}).ToList();
			return View(party);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			var party = await _context.Parties.FindAsync(id);
			if (party != null)
			{
				_context.Parties.Remove(party);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool PartyExists(Guid id)
		{
			return _context.Parties.Any(e => e.Id == id);
		}
	}
}
