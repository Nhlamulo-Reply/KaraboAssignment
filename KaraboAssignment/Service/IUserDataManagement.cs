﻿using KaraboAssignment.Data;
using KaraboAssignment.Models;

namespace KaraboAssignment.Service
{
    public interface IUserDataManagement
    {
        public Task<ApplicationUser> GetUserByEmail(string email);

        public Task<ApplicationUser> GetUserByUserId(string userId);

        public Task<string> GetUserRole(string userId);

        public Task<string> GetUserRoleByEmail(string email);

        public Task<string> CreateUser(UserDetailsViewModel userViewModel);

        public Task<string> CreateUser(RegisterViewModel registerViewModel);

        public Task<Guid> CreatFarmer(Farmer farmer);

        public Task EditUser(UserDetailsViewModel userViewModel);

       // public Task UpdateUserAccountStatus(string userId, AccountStatus accountStatus);

        public Task SuperDeleteUser(string userId);

        public Task<List<UserDetailsViewModel>> GetAllUsersDetails();
    }
}
