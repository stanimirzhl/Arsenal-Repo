using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieData.Data
{
	public class Movie
	{
		[Key]
		public int Int { get; set; }

		public string Title { get; set; } = null!;

		public string Director { get; set; } = null!;

		public int Year { get; set; }

		public string Genre { get; set; } = null!;
	}
}
