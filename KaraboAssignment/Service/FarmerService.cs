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
        public FarmerService(ApplicationDbContext context, UserManager<IdentityUser> UserManager, ILogger logger)
        {
          _context = context;
          _userManager = UserManager;
          _logger = logger;
        
        }
        public Task AddProductAsync(ProductViewModel product)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductViewModel>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProductViewModel> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
