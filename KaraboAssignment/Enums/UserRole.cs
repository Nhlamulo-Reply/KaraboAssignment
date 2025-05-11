using System.ComponentModel.DataAnnotations;

namespace KaraboAssignment.Enums
{
      
        public enum UserRole
        {

            [Display(Name = "Farmers")]
            Farmers = 1,

            [Display(Name = "Employees")]
            Employees = 2
    }
   
}
