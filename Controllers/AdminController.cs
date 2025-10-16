using Microsoft.AspNetCore.Mvc;
using SIMS.Data;

namespace SIMS.Controllers
{
    public class AdminController : Controller
    {

        private readonly DatabaseHelper _db;

        public AdminController(DatabaseHelper db)
        {
            _db = db;
        }
        
        
        public IActionResult Index()
        {
            return View();
        }



        public IActionResult StudentList()
        {

            return View();
        }

        public IActionResult PeopleList() 
        { 
            var people = _db.GetAllPeople();
            return View("PeopleList",people); 
        }
    }
}
