using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    public class PeopleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
