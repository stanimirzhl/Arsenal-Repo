using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsProject.Data.Models
{
	public class Cat
	{
		[Key]
		public Guid Id { get; set; }

		public string Name { get; set; }

		public int Age { get; set; }

		public string ImageUrl { get; set; }

		public Breed? Breed { get; set; }

		[ForeignKey(nameof(Breed))]
		public Guid? BreedId { get; set; }
	}
}
