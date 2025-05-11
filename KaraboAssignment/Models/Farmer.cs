using System.ComponentModel.DataAnnotations;

namespace KaraboAssignment.Models
{
    public class Farmer
    {
        [Key]
        public Guid FarmerId { get; set; }

        public string? UserId { get; set; } 

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Navigation property to Products
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
