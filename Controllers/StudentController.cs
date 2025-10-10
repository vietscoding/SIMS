using Microsoft.AspNetCore.Mvc;
using SIMS.Models;
using SIMS.Services;

namespace SIMS.Controllers
{
    public class StudentController : Controller
    {
        //// Giả lập database
        //private static List<Student> students = new List<Student>
        //{
        //    new Student{"1", "Alice", DateTime.Now},
        //    new Student{ StudentId="2", FullName = "Bob", DateOfBirth=DateTime.Now}
        //};

        //public IActionResult Index()
        //{
        //    return View(students);
        //}

        //public IActionResult Details(int id)
        //{
        //    var student = students.Find(s => s.Id.Equals(id));
        //    return View(students);
        //}


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
            Student student = new("Id1", "student1",DateTime.Now);
            return View(student);
        }

    }
}

