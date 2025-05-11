using KaraboAssignment.ViewModels;

namespace KaraboAssignment.Service
{
    public interface IFarmerService
    {
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();
        Task AddProductAsync(ProductViewModel product);
        Task DeleteProductAsync(int id);
        Task<ProductViewModel> GetProductByIdAsync(int id);

        // For Employees
        public Task AddFarmerAsync(Farmer farmer);
       public  Task<List<Farmer>> GetAllFarmersAsync();
       public Task<Farmer?> GetFarmerByIdAsync(Guid farmerId);

    }
}
