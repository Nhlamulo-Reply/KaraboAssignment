using KaraboAssignment.Data;
using KaraboAssignment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KaraboAssignment.Controllers
{
    [Authorize(Roles = "Farmer")]


    public class FarmerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FarmerController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> ListProducts()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");

            }

            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == user!.Email);



           // var products = _context.Products.Where(p => p.FarmerId == farmer!.FarmerId);
            return View(farmer);
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
          //  product.FarmerId = farmer.FarmerId;



            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Product added successfully!";
            return RedirectToAction("AddProducts", "Dashboard");
        }
    }
}