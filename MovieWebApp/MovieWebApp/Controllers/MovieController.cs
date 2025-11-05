using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieData;
using MovieData.Data;

namespace MovieWebApp.Controllers
{
	public class MovieController : Controller
	{
		private readonly MovieDbContext context;

		public MovieController(MovieDbContext context)
		{
			this.context = context;
		}

		public async Task<IActionResult> Index()
		{
			return View(await context.Movies.ToListAsync());
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var movie = await context.Movies.FindAsync(id);

			if (movie == null)
			{
				return NotFound();
			}

			return View(movie);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create([Bind("Title,Director,Year,Genre")] Movie movie)
		{
			if (ModelState.IsValid)
			{
				context.Add(movie);
				await context.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(movie);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var movie = await context.Movies.FindAsync(id);
			if (movie == null)
			{
				return NotFound();
			}
			return View(movie);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, [Bind("Title,Director,Year,Genre")] Movie movie)
		{
			if (id != movie.Int)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				var model = await context.Movies.FindAsync(id);

				if(model is null)
				{
					return NotFound();
				}

				context.Update(movie);
				await context.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			return View(movie);
		}


		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var movie = await context.Movies.FindAsync(id);
			if (movie == null)
			{
				return NotFound();
			}

			return View(movie);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var movie = await context.Movies.FindAsync(id);
			if (movie != null)
			{
				context.Movies.Remove(movie);
			}

			await context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
	}
}
