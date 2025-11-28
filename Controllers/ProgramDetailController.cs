using Microsoft.AspNetCore.Mvc;
using SIMS.Data;
using SIMS.Models;
using SIMS.ViewModels;
using System;
using System.Linq;

namespace SIMS.Controllers
{
    public partial class ProgramDetailController : Controller
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

        // POST: /ProgramDetail/AddCurriculum
        [HttpPost]
        public IActionResult AddCurriculum([FromBody] CurriculumCreateModel model)
        {
            if (model == null)
                return BadRequest(new { success = false, message = "Invalid request payload." });

            if (model.ProgramId <= 0 || model.CourseId <= 0)
                return BadRequest(new { success = false, message = "ProgramId and CourseId must be positive integers." });

            var program = _db.GetAcademicProgramById(model.ProgramId);
            if (program == null || program.AcademicProgramId == 0)
                return NotFound(new { success = false, message = "Program not found." });
             //|| course.CourseId == 0
            var course = _db.GetCourseById(model.CourseId);
            if (course == null)
                return NotFound(new { success = false, message = "Course not found." });

            // Prevent duplicate (non-deleted) curriculum entry
            var existing = _db.GetAllCurriculumsByProgramId(model.ProgramId)
                              .FirstOrDefault(c => (c.CourseId ?? 0) == model.CourseId && !(c.IsDeleted ?? false));
            if (existing != null)
                return Conflict(new { success = false, message = "This course is already part of the program." });

            var curriculum = new Curriculum
            {
                ProgramId = model.ProgramId,
                CourseId = model.CourseId,
                IsElective = false,
                IsBeforeCapstoneProject = false,
                IsPrerequisiteCapstoneProject = false,
                IsDeleted = false,
                CreatedAt = DateTime.Now
            };

            var added = _db.AddCurriculum(curriculum);
            if (!added)
                return StatusCode(500, new { success = false, message = "Failed to add course to program. Please try again." });

            return Json(new { success = true, message = "Course added to program.", curriculumId = curriculum.CurriculumId });
        }

        // POST: /ProgramDetail/DeleteCurriculum
        [HttpPost]
        public IActionResult DeleteCurriculum([FromBody] int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid curriculum id." });
            }

            var existing = _db.GetCurriculumById(id);
            if (existing == null || existing.CurriculumId == 0)
            {
                return NotFound(new { success = false, message = "Curriculum not found." });
            }

            // Optional: ensure the curriculum belongs to the program shown (extra safety)
            // if (existing.ProgramId != expectedProgramId) { ... }

            var removed = _db.RemoveCurriculum(id);
            if (!removed)
            {
                return StatusCode(500, new { success = false, message = "Failed to delete curriculum. Please try again." });
            }

            return Json(new { success = true, message = "Curriculum removed from program." });
        }
    }
}
