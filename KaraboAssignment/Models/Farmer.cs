using System.ComponentModel.DataAnnotations;

namespace KaraboAssignment.Models
{
    public class Farmer
    {
        public string FarmerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }

       // public ICollection<Product> Products { get; set; }
    }
}
