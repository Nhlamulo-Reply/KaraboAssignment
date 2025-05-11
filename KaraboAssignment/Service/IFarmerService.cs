using KaraboAssignment.Models;


namespace KaraboAssignment.Service
{
    public interface IFarmerService
    {
      
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<Product> GetProductByIdAsync(int id);

        // For Employees
        public Task AddFarmerAsync(Farmer farmer);
        public Task<List<Farmer>> GetAllFarmersAsync();
       public Task<Farmer?> GetFarmerByIdAsync(Guid farmerId);

    }
}
