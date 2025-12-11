using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PresentGivers.Models
{
	public class GiftViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int CategoryId { get; set; } = 0;

		public string? CategoryName { get; set; }

		[Url]
		public string ImageUrl { get; set; }

		public bool IsTaken { get; set; }

		public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
	}
}
