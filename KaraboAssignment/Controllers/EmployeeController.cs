using KaraboAssignment.Data;
using KaraboAssignment.Service;
using KaraboAssignment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace KaraboAssignment.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;
        private readonly IUserDataManagement _usersIO;
        private readonly ILogger<AccountController> _logger;
    
           

            public EmployeeController(ApplicationDbContext context, IProductService productService, ILogger<AccountController> logger, IUserDataManagement usersIO)
        {
            _context = context;
            _productService = productService;
            _logger = logger;
            _usersIO = usersIO;
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

            _logger.LogError($"Farmer  {farmer}");

            if (ModelState.IsValid)
            {
               
                IDbContextTransaction transaction = _context.Database.BeginTransaction();
                try
                {
                    var userId = await _usersIO.CreatFarmer(farmer);


                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError($"There was an error registering new account with email {farmer.Name}", ex);
                }
              
                return RedirectToAction("AdminIndex", "Dashboard");
            }
            return View();
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
