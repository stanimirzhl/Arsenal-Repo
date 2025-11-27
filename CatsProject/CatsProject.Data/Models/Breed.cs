using System.ComponentModel.DataAnnotations;

namespace CatsProject.Data.Models
{
	public class Breed
	{
		[Key]
		public Guid Id { get; set; }

		public string Title { get; set; }

		public ICollection<Cat> Cats { get; set; } = new HashSet<Cat>();
	}
}
