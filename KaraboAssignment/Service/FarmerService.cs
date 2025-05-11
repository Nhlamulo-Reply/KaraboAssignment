using KaraboAssignment.Data;
using KaraboAssignment.Models;
using Microsoft.AspNetCore.Identity;

namespace KaraboAssignment.Service
{
    public class FarmerService : IFarmerService
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private UserManager<IdentityUser> _userManager;
        public FarmerService(ApplicationDbContext context, UserManager<IdentityUser> UserManager, ILogger logger)
        {
          _context = context;
          _userManager = UserManager;
          _logger = logger;
        
        }

        public Task AddFarmerAsync(Farmer farmer)
        {
            throw new NotImplementedException();
        }

 

        public Task AddProductAsync(Models.Product product)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Farmer>> GetAllFarmersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Farmer?> GetFarmerByIdAsync(Guid farmerId)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<Farmer>> IFarmerService.GetAllFarmersAsync()
        {
            throw new NotImplementedException();
        }

        Task<Models.Farmer?> IFarmerService.GetFarmerByIdAsync(Guid farmerId)
        {
            throw new NotImplementedException();
        }

        Task<Models.Product> IFarmerService.GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
