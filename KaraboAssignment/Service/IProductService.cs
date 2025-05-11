using KaraboAssignment.Models;

namespace KaraboAssignment.Service
{
    public interface IProductService
    {
        Task AddProductAsync(Product product);
        Task<List<Product>> GetAllProducts();
        Task<List<Product>> GetFarmerProductsAsync(Guid farmerId);
        Task<List<Product>> FilterProductsAsync(string? category, DateTime? start, DateTime? end, Guid? farmerId);
    }
}
