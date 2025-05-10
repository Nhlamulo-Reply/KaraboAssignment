using KaraboAssignment.ViewModels;

namespace KaraboAssignment.Service
{
    public interface IProductService
    {
        Task AddProductAsync(Product product);
        Task<IEnumerable<Product>> GetFarmerProductsAsync(Guid farmerId); 
        Task<IEnumerable<Product>> FilterProductsAsync(string category, DateTime? start, DateTime? end, Guid? farmerId);
    }

}
