using System.ComponentModel.DataAnnotations;

namespace KaraboAssignment.ViewModels
{
    public class Farmer
    {
        public int FarmerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
