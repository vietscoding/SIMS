using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Đừng làm thế này ở production nhé.
            if (username == "student" && password == "123")
            {
                return RedirectToAction("Index", "Student");
            }
            else if (username == "registrar" && password == "123")
            {
                return RedirectToAction("Index", "Registrar");
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

    }
}
