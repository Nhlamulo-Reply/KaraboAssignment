using Microsoft.AspNetCore.Mvc;

namespace KaraboAssignment.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
