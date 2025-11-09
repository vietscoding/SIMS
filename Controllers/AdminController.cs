using Microsoft.AspNetCore.Mvc;
using SIMS.Data;
using SIMS.ViewModels;
using SIMS.Models;

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
            var students = _db.GetAllStudents();
            return View("StudentList",students);
        }

        public IActionResult StudentList2()
        {
            var studentDetailsList = new StudentDetailsViewModel
            {
                Students = _db.GetAllStudents(),
                Programs = _db.GetAllAcademicPrograms()
            };
            return View("StudentList2", studentDetailsList);
        }

        public IActionResult PeopleList() 
        {

            var people = _db.GetAllPeople();
            return View("PeopleList", people);
        }



        public IActionResult AcademicProgram()
        {
            return View();
        }
    }
}
