using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SIMS.Data;
using SIMS.Models;
using SIMS.Services;

namespace SIMS.Controllers
{
    public class StudentController : Controller
    {
        private readonly DatabaseHelper _dbHelper;

        public StudentController(IConfiguration configuration)
        {
            _dbHelper = new DatabaseHelper(configuration);
        }

        public IActionResult Index()
        {
            return View();
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
            var student = _dbHelper.GetStudentById(id);
            if (student == null)
                return NotFound();
            return Json(student);
        }

        [HttpPost]
        public IActionResult UpdateStudent(Student student)
        {
            var result = _dbHelper.UpdateStudent(student);
            if (!result)
                return BadRequest("Update failed.");
            return Ok();
        }


    }
}

