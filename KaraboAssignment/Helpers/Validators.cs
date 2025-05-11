using System.Text.RegularExpressions;
using System.Linq;

namespace KaraboAssignment.Helpers
{
    public class Validators
    {


        public static bool IsValidPassword(string password)
        {
            bool length = password.Length > 7;
            bool containsLowerLetter = Regex.Matches(password, @"[a-z]").Count > 0;
            bool containsUpperLetter = Regex.Matches(password, @"[A-Z]").Count > 0;
            bool containsNumber = password.Any(x => char.IsNumber(x));
            bool containsSpecialChar = password.Any(x => !char.IsLetterOrDigit(x));

            return length && containsLowerLetter && containsUpperLetter && containsNumber && containsSpecialChar;
        }

        public static bool IsValidEmail(string email)
        {

            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public static bool IsValidCellphone(string cellphone)
        {
          return Regex.IsMatch(cellphone, @"^(0\d{9}|\+27\d{9})$");
        }
    }
}
