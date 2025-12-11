using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Givers.Data.Models
{
	public class Gift
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }
		public string ImageUrl { get; set; }

		[ForeignKey(nameof(Category))]
		public int CategoryId { get; set; }

		public Category Category { get; set; }

		public bool IsTaken { get; set; } = false;
	}
}
