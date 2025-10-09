using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    public class RegistrarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
