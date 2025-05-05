using KaraboAssignment.ViewModels;

namespace KaraboAssignment.Service
{
    public interface IProductService
    {
        Task AddProductAsync(Product product);
        Task<IEnumerable<Product>> GetFarmerProductsAsync(int farmerId); // <- Ensure this
        Task<IEnumerable<Product>> FilterProductsAsync(string category, DateTime? start, DateTime? end, int? farmerId);
    }

}
