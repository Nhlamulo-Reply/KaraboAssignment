using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KaraboAssignment.Models
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; } = Guid.NewGuid(); 

        [Required]
        public string? ProductName { get; set; }

        [Required]
        public string? Category { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Production Date")]
        public DateTime ProductionDate { get; set; }
         
        public string? Description { get; set; }
            
        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Foreign key
        [ForeignKey("Farmer")]
        public Guid FarmerId { get; set; }

        // Navigation property
        public Farmer? Farmer { get; set; }
    }
}
