using KaraboAssignment.Data;
using KaraboAssignment.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace KaraboAssignment.Service
{
    public class FarmerService : IFarmerService
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private UserManager<IdentityUser> _userManager;
        public FarmerService(ApplicationDbContext context, UserManager<IdentityUser> UserManager)
        {
          _context = context;
          _userManager = UserManager;
        
        }
        public Task AddProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
