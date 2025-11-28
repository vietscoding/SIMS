using Microsoft.AspNetCore.Mvc;
using SIMS.Data;
using SIMS.Models;
using SIMS.ViewModels;
using System.Linq;

namespace SIMS.Controllers
{
    public class ProgramDetailController : Controller
    {
        private readonly DatabaseHelper _db;
        public ProgramDetailController(DatabaseHelper db)
        {
            _db = db;
        }

        // --- New action: ProgramDetail ---
        public IActionResult ProgramDetail(int id)
        {
            if (id <= 0) return BadRequest();

            var program = _db.GetAcademicProgramById(id);

            if (program == null || program.AcademicProgramId == 0)
            {
                return NotFound();
            }

            var curriculums = _db.GetAllCurriculumsByProgramId(id);

            // Aggregate all CourseDependency entries that belong to any curriculum in the program
            var courseDependencies = curriculums
                .SelectMany(c => _db.GetAllCourseDependenciesByCurriculumId(c.CurriculumId))
                .ToList();

            // ProgramDetail.cshtml expects an enumerable (it's currently iterating Model).
            var model = new AcademicProgramDetailViewModel()
            {
                AcademicProgram = program,
                Curriculums = curriculums,
                Major = program.Major,
                Majors = _db.GetAllMajors(),
                Faculties = _db.GetAllFaculties(),
                CourseDependencies = courseDependencies
            };

            return View("Views/Admin/ProgramDetail.cshtml", model);
        }

    }
}
