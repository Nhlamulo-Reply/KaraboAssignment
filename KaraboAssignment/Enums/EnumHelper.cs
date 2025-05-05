using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace KaraboAssignment.Enums
{
    public static class EnumHelper
    {
        /// <summary>
        /// Display the name set in annotation DisplayName on top of enum value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum value)
        {
            return value.GetType()?
           .GetMember(value.ToString())?.First()?
           .GetCustomAttribute<DisplayAttribute>()?
           .Name;
        }
    }
}
