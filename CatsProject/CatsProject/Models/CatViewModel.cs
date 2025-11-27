using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CatsProject.Models
{
	public class CatViewModel
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public int Age { get; set; }

		[Url(ErrorMessage = "Needs to be url")]
		public string ImageUrl { get; set; }

		public string? BreedName { get; set; }

		public List<SelectListItem> Breeds { get; set; } = new List<SelectListItem>();

		public Guid? BreedId { get; set; }
	}
}
