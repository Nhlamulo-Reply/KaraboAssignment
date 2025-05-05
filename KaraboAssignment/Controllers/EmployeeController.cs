using KaraboAssignment.Data;
using KaraboAssignment.Service;
using KaraboAssignment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KaraboAssignment.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;
        public ApplicationDbContext Context => _context;


        public EmployeeController(ApplicationDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string category, DateTime? startDate, DateTime? endDate, int? farmerId)
        {
            ViewBag.Farmers = await _context.Farmers.ToListAsync();

            var products = await _productService.FilterProductsAsync(category, startDate, endDate, farmerId);

            // Make sure to return an empty list if null
            return View(products ?? new List<Product>());
        }



        /* [Authorize(Roles = "Employee")]*/



        [HttpGet]
        public IActionResult AddFarmer()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFarmer(Farmer farmer)
        {
            if (!ModelState.IsValid) return View(farmer);

            Context.Farmers.Add(farmer);
            await Context.SaveChangesAsync();

            return RedirectToAction("AddFarmer");
        }

        [HttpGet]
        public async Task<IActionResult> FilterProducts(string category, DateTime? startDate, DateTime? endDate, int? farmerId)
        {
            ViewBag.Farmers = await _context.Farmers.ToListAsync();
            var products = await _productService.FilterProductsAsync(category, startDate, endDate, farmerId);
            return View(products);
        }
    }

}
