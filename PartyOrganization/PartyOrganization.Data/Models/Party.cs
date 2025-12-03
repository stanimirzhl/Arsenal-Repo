using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyOrganization.Data.Models
{
	public class Party
	{
		[Key]
		public Guid Id { get; set; }

		public string Title { get; set; }

		public DateTime Date { get; set; }

		public string Description { get; set; }

		public string ImageUrl { get; set; }

		[ForeignKey(nameof(Location))]
		public Guid LocationId { get; set; }

		public Location Location { get; set; }

		public Organizer Organizer { get; set; }

		[ForeignKey(nameof(Organizer))]
		public Guid OrganizerId { get; set; }
	}
}
