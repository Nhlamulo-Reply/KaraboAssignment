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
    public class DashBoardController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;
        private readonly IUserDataManagement _usersIO;
        private readonly ILogger<AccountController> _logger;



        public DashBoardController(ApplicationDbContext context, IProductService productService, ILogger<AccountController> logger, IUserDataManagement usersIO)
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

        [HttpPost]
        public IActionResult AddFarmer()
        {
            return View();
        }
        public IActionResult AddProducts()
        {
            return View();
        }

        public IActionResult FilterProducts()
        {
            return View();
        }
    }
}
