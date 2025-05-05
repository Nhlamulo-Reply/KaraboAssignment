using Microsoft.AspNetCore.Mvc;

namespace KaraboAssignment.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
