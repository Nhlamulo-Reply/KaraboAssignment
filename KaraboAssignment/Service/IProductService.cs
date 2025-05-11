using KaraboAssignment.Models;

namespace KaraboAssignment.Service
{
    public interface IProductService
    {

        // For Employees
        public Task<List<Product>> GetAllProducts();
        public Task<List<Product>> FilterProductsAsync(string? category, DateTime? start, DateTime? end, Guid? farmerId);
          
        // For Farmers
       public  Task AddProductAsync(Product product);
       public Task<List<Product>> GetFarmerProductsAsync(Guid farmerId);
           
    }

}
