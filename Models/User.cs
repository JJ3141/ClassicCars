using System.ComponentModel.DataAnnotations;
namespace ClassicCars.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string? Username { get; set; }
		[Required]
		public string? Password { get; set; }

		public virtual ICollection<Car> Cars { get; set; }
			= new HashSet<Car>();
	}
}

