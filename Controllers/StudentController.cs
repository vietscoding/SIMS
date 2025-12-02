using Microsoft.AspNetCore.Mvc;
using SIMS.Data;
using SIMS.Models;
using SIMS.ViewModels;

namespace SIMS.Controllers
{
    public class StudentController : Controller
    {
        private readonly DatabaseHelper _db;
        public StudentController(DatabaseHelper db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View("~/Index.cshtml");
        }

        public IActionResult StudentList()
        {
            var students = _db.GetAllStudents();
            return View("StudentList", students);
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

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetStudentById(int id)
        {
            var student = _db.GetStudentById(id);
            if (student == null || student.StudentId == 0)
                return NotFound();
            return Json(student);
        }

        [HttpPost]
        public IActionResult UpdateStudent(Student student)
        {
            var result = _db.UpdateStudent(student);
            if (!result)
                return BadRequest("Update failed.");
            return Ok();
        }


    }
}

