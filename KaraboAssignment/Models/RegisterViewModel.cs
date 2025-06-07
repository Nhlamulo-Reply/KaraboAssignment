using System.ComponentModel.DataAnnotations;

namespace KaraboAssignment.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string? Firstname { get; set; }

        [Required]
        public string? Lastname { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]

        [Compare("Password")]
        public string? ConfirmPassword { get; set; }

        public bool Terms { get; set; }
    }

}
