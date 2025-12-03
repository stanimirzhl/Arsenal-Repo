using Microsoft.AspNetCore.Mvc.Rendering;

namespace PartyOrganization.Models
{
	public class PartyViewModel
	{
		public Guid Id { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string Date { get; set; }

		public string Image { get; set; }

		public string? LocationName { get; set; }

		public string? OrganizerName { get; set; }

		public Guid LocationId { get; set; }

		public Guid OrganizerId { get; set; }

		public List<SelectListItem> Locations { get; set; } = new List<SelectListItem>();

		public List<SelectListItem> Organizers { get; set; } = new List<SelectListItem>();
	}
}
