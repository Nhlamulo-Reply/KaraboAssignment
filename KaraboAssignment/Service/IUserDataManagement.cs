using KaraboAssignment.Data;
using KaraboAssignment.Models;

namespace KaraboAssignment.Service
{
    public interface IUserDataManagement
    {
        public Task<ApplicationUser> GetUserByEmail(string email);

        public Task<ApplicationUser> GetUserByUserId(string userId);

        public Task<string> GetUserRoleByEmail(string email);

       public Task<Guid> CreatFarmer(RegisterViewModel farmer);


    }
}
