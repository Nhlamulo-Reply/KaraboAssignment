using System.ComponentModel.DataAnnotations;

namespace KaraboAssignment.ViewModels
{
    public class Product
    {
        public int? ProductId { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Category { get; set; }
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }

        public int? FarmerId { get; set; }
        public Farmer Farmer { get; set; }
    }
}
