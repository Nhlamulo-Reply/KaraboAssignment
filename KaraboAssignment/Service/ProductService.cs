using KaraboAssignment.Data;
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

        public async Task<IEnumerable<Product>> GetFarmerProductsAsync(int farmerId)
        {
            return await _context.Products
                .Include(p => p.Farmer)
                .Where(p => p.FarmerId == farmerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> FilterProductsAsync(string category, DateTime? start, DateTime? end, int? farmerId)
        {
            var query = _context.Products.Include(p => p.Farmer).AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category.Contains(category));

            if (start.HasValue && end.HasValue)
                query = query.Where(p => p.ProductionDate >= start && p.ProductionDate <= end);

            if (farmerId.HasValue)
                query = query.Where(p => p.FarmerId == farmerId);

            return await query.ToListAsync();
        }
    }


}
