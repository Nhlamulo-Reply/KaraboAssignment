using KaraboAssignment.Data;

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



    }
}