using Microsoft.AspNetCore.Mvc;

namespace KaraboAssignment.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AdminIndex()
        {
            return View();
        }
        public IActionResult AddFarmer()
        {
            return View();
        }
        public IActionResult AddProducts()
        {
            return View();
        }
    }
}
