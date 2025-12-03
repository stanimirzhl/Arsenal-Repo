using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyOrganization.Data.Models
{
	public class Organizer
	{
		[Key]
		public Guid Id { get; set; }

		public string Name { get; set; }

		public int PhoneNumber { get; set; }

		public ICollection<Party> Parties { get; set; } = new HashSet<Party>();
	}
}
