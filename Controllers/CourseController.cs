using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
