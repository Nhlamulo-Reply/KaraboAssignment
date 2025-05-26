using KaraboAssignment.Data;
using KaraboAssignment.Service;
using KaraboAssignment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using KaraboAssignment.Helpers;
using KaraboAssignment.Enums;


namespace KaraboAssignment.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IProductService _productService;
        private readonly IUserDataManagement _usersIO;
        private readonly ILogger<DashBoardController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashBoardController(
            ApplicationDbContext context,
            IProductService productService,
            ILogger<DashBoardController> logger,
            IUserDataManagement usersIO,
              UserManager<ApplicationUser> userManager)
        {
            _dbContext = context;
            _productService = productService;
            _logger = logger;
            _usersIO = usersIO;
            _userManager = userManager;
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
        public async Task<IActionResult> AddFarmer(Farmer farmer)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogError("Model error in field {Field}: {ErrorMessage}", state.Key, error.ErrorMessage);
                    }
                }
                return View("AddFarmer", farmer); // corrected view name
            }

            if (!string.IsNullOrEmpty(farmer.PhoneNumber) &&
                !Validators.IsValidCellphone(farmer.PhoneNumber))
            {
                ModelState.AddModelError(string.Empty, "Enter a valid South African phone number.");
                return View("AddFarmer", farmer); // corrected view name
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var farmerId = await _usersIO.CreatFarmer(farmer);
                _logger.LogInformation("Successfully created farmer with ID: {FarmerId}", farmerId);
                await transaction.CommitAsync();

                TempData["Success"] = "Farmer added successfully!";
                return RedirectToAction("AddFarmer", "Dashboard");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error registering farmer with email: {Email}", farmer.Email);
                ModelState.AddModelError("", "An error occurred while registering the farmer.");
                return View("AddFarmer", farmer); // corrected view name
            }
        }



        [HttpGet]
        public IActionResult AddProducts()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product productModel)
        {
            if (!ModelState.IsValid)
                return View("AddProducts", productModel);

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    return View("AddProducts", productModel);
                }

                // Find or create Farmer
                var farmer = await _dbContext.Farmers
                    .FirstOrDefaultAsync(f => f.IdentityUserId == user.Id);

                if (farmer == null)
                {
                    farmer = new Farmer
                    {
                        FarmerId = Guid.NewGuid(),
                        IdentityUserId = user.Id,
                        Name = user.UserName ?? "Unknown",
                        Email = user.Email
                    };

                    _dbContext.Farmers.Add(farmer);
                    await _dbContext.SaveChangesAsync();
                }

                // Assign FarmerId to the product
                productModel.FarmerId = farmer.FarmerId;

                await _productService.AddProductAsync(productModel);
                await transaction.CommitAsync();

                TempData["Success"] = "Product added successfully!";
                return RedirectToAction("AddProducts", "Dashboard");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error adding product.");
                ModelState.AddModelError("", "An error occurred while adding the product.");
                return View("AddProducts", productModel);
            }
        }







        public async Task<IActionResult> ViewProduct()
        {
            // Get the logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Match user with Farmer
            var farmer = await _dbContext.Farmers
                 .FirstOrDefaultAsync(f => f.IdentityUserId == user.Id);
            if (farmer == null)
            {
                TempData["Error"] = "Farmer profile not found.";
                return RedirectToAction("Index", "Home");
            }

            // Get the farmer's products using the service method
            var products = await _productService.GetFarmerProductsAsync(farmer.FarmerId);

            return View("ViewProduct", products);
        }

        [HttpGet]
        public async Task<IActionResult> FilterProducts(string? category, DateTime? startDate, DateTime? endDate, Guid? farmerId)
        {
            var products = await _productService.FilterProductsAsync(category, startDate, endDate, farmerId);

            //ViewBag.Farmers = await _farmerService.GetAllFarmersAsync();

            return View(products); // Pass filtered product list to the view
        }

    }
}
