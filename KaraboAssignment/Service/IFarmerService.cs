using KaraboAssignment.ViewModels;

namespace KaraboAssignment.Service
{
    public interface IFarmerService
    {
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();
        Task AddProductAsync(ProductViewModel product);
        Task DeleteProductAsync(int id);
        Task<ProductViewModel> GetProductByIdAsync(int id);
     
    }
}
