using KaraboAssignment.Data;
using KaraboAssignment.Models;
using KaraboAssignment.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KaraboAssignment.Service
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<Product>> FilterProductsAsync(string category, DateTime? start, DateTime? end, Guid? farmerId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetFarmerProductsAsync(Guid farmerId)
        {
            throw new NotImplementedException();
        }

        Task<List<Product>> IProductService.FilterProductsAsync(string? category, DateTime? start, DateTime? end, Guid? farmerId)
        {
            throw new NotImplementedException();
        }

        /* public async Task<IEnumerable<Product>> GetFarmerProductsAsync(Guid farmerId)
        {
            return await _context.Products
                .Include(p => p.FarmerName)
                .Where(p => p.FarmerId == farmerId)
                .ToListAsync();
        }



       public async Task<IEnumerable<Product>> IProductService.FilterProductsAsync(string category, DateTime? start, DateTime? end, Guid? farmerId)
        {
            var query = _context.Products.Include(p => p.FarmerName).AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category.Contains(category));

            if (start.HasValue && end.HasValue)
                query = query.Where(p => p.ProductionDate >= start && p.ProductionDate <= end);

            if (farmerId.HasValue)
                query = query.Where(p => p.FarmerId == farmerId.Value);

            return await query.ToListAsync();
        }*/
    }


}
