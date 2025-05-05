using System.ComponentModel.DataAnnotations;

namespace KaraboAssignment.Enums
{
      
        public enum UserRole
        {
            [Display(Name = "Admin")]
            Admin = 1,

            [Display(Name = "Client")]
            Client = 2,

            [Display(Name = "Farmers")]
            Farmers = 3,

            [Display(Name = "Employees")]
            Employees = 4
    }
   
}
