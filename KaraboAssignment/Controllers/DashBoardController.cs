using KaraboAssignment.Data;
using KaraboAssignment.Service;
using KaraboAssignment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;


namespace KaraboAssignment.Controllers
{
    public class DashBoardController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;
        /*private readonly IFarmerService _farmerService;*/
        private readonly IUserDataManagement _usersIO;
        private readonly ILogger<DashBoardController> _logger;
        private readonly UserManager<IdentityUser> _userManager;



        public DashBoardController(ApplicationDbContext context, IProductService productService, ILogger<DashBoardController> logger, IUserDataManagement usersIO)
        {
            _context = context;
            _productService = productService;
            _logger = logger;
            _usersIO = usersIO;

        }



        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AdminIndex()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AddFarmer()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFarmer(Models.Farmer farmer)
        {

            _logger.LogError($"Farmer  {farmer}");

            if (ModelState.IsValid)
            {

                IDbContextTransaction transaction = _context.Database.BeginTransaction();
                try
                {
                    //Add Farmer into the db function
                    var userId = await _usersIO.CreatFarmer(farmer);

                    _logger.LogInformation("Created farmer {Id}", userId);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError($"There was an error registering new account with email {farmer.Email}", ex);
                }

                return RedirectToAction("AdminIndex", "Dashboard");
            }
            return View();
        }

        [HttpGet]
        public IActionResult AddProducts()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddProducts(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            {
                _logger.LogWarning("Products from the form", product);
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    return View(product);
                }

                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == user.Email);
                if (farmer == null)
                {
                    ModelState.AddModelError("", "Farmer account not found.");
                    return View(product);
                }

                var products = new Product
                {
                    ProductName = product.ProductName,
                    FarmerId = farmer.FarmerId,
                    

                };

                _logger.LogWarning("All products to be added  from the form", products);
                _context.Products.Add(products);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Product added successfully!";
                return RedirectToAction("AddProducts", "Dashboard");
            }



          
        }
    }
}
