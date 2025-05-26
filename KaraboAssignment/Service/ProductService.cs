using KaraboAssignment.Data;
using KaraboAssignment.Models;
using Microsoft.EntityFrameworkCore;

namespace KaraboAssignment.Service
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ApplicationDbContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddProductAsync(Product model)
        {
            try
            {
                _logger.LogInformation("Adding product with FarmerId: {FarmerId}", model.FarmerId);

                var product = new Product
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = model.ProductName,
                    Category = model.Category,
                    ProductionDate = model.ProductionDate,
                    Description = model.Description,
                    Quantity = model.Quantity,
                    FarmerId = model.FarmerId,
                    CreatedAt = DateTime.Now
                };

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Product added successfully with ID: {ProductId}", product.ProductId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add product with FarmerId: {FarmerId}", model.FarmerId);
                throw;
            }
        }



        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products
                .Include(p => p.Farmer)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Product>> GetFarmerProductsAsync(Guid farmerId)
        {
            return await _context.Products
                .Include(p => p.Farmer)
                .Where(p => p.FarmerId == farmerId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Product>> FilterProductsAsync(string? category, DateTime? start, DateTime? end, Guid? farmerId)
        {
            var query = _context.Products
                .Include(p => p.Farmer)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(p => p.Category.Contains(category));

            if (start.HasValue)
                query = query.Where(p => p.ProductionDate >= start.Value);

            if (end.HasValue)
                query = query.Where(p => p.ProductionDate <= end.Value);

            if (farmerId.HasValue)
                query = query.Where(p => p.FarmerId == farmerId.Value);

            return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }
    }
}
