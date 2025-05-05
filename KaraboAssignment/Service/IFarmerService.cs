using KaraboAssignment.ViewModels;

namespace KaraboAssignment.Service
{
    public interface IFarmerService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<Product> GetProductByIdAsync(int id);
    }
}
