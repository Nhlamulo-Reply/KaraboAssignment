using System.ComponentModel.DataAnnotations;

namespace KaraboAssignment.Models
{
    public class Product
    {
        public int? ProductId { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Category { get; set; }
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }

        public Farmer? Farmer { get; set; }

        public Guid Id { get; set; }
             

        [DataType(DataType.Date)]
        [Display(Name = "Production Date")]
  
        public DateTime? ExpiryDate { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [Display(Name = "Farmer")]
        public string FarmerName { get; set; }

        public Guid FarmerId { get; set; }


    }
}
