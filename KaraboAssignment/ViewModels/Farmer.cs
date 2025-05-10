using System.ComponentModel.DataAnnotations;

namespace KaraboAssignment.ViewModels
{
    public class Farmer
    {
        [Key]
        public Guid FarmerId { get; set; } = Guid.NewGuid();

     
        public string? Name { get; set; }

   
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
