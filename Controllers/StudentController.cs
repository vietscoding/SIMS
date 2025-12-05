using Microsoft.AspNetCore.Mvc;
using SIMS.Data;
using SIMS.Models;
using SIMS.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

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

        public IActionResult StudentList(int page = 1)
        {
            int pageSize = 10;
            var query = _db.GetAllStudents()
                .Include(s => s.Program)
                .Include(s => s.Person);

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems/ pageSize);

            var data = query
                .OrderBy(s => s.StudentId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new StudentDetailsViewModel
            {
                Students = data,
                Programs = _db.GetAllAcademicPrograms()
            };

            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;

            return View(result);

        }
        //public IActionResult StudentList2()
        //{
        //    var studentDetailsList = new StudentDetailsViewModel
        //    {
        //        Students = _db.GetAllStudents(),
        //        Programs = _db.GetAllAcademicPrograms()
        //    };
        //    return View("StudentList2", studentDetailsList);
        //}

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

