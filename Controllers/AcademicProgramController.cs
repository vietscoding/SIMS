using Microsoft.AspNetCore.Mvc;
using SIMS.Data;
using SIMS.ViewModels;
using SIMS.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace SIMS.Controllers
{
    public class AcademicProgramController : Controller
    {
        private readonly DatabaseHelper _db;
        public AcademicProgramController(DatabaseHelper db)
        {
            _db = db;
        }
        public IActionResult AcademicProgram()
        {
            var academicProgramList = new AcademicProgramViewModel
            {
                AcademicPrograms = _db.GetAllAcademicPrograms(),
                Majors = _db.GetAllMajors(),
                Faculties = _db.GetAllFaculties(),
            };
            return View("Views/Admin/AcademicProgram.cshtml", academicProgramList);
        }

        [HttpPost]
        public IActionResult AddAcademicProgram([FromBody] AcademicProgram model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid request payload." });
            }

            if (string.IsNullOrWhiteSpace(model.AcademicProgramName))
            {
                return BadRequest(new { message = "Program name is required." });
            }

            if (!model.NumberOfSemester.HasValue || model.NumberOfSemester <= 0)
            {
                return BadRequest(new { message = "Number of semesters must be a positive integer." });
            }

            // If faculty/major provided, ensure they exist
            if (model.FacultyId.HasValue)
            {
                var fac = _db.GetFacultyById(model.FacultyId.Value);
                if (fac == null || fac.FacultyId == 0)
                {
                    return BadRequest(new { message = "Specified faculty does not exist." });
                }
            }

            if (model.MajorId.HasValue)
            {
                var maj = _db.GetMajorById(model.MajorId.Value);
                if (maj == null || maj.MajorId == 0)
                {
                    return BadRequest(new { message = "Specified major does not exist." });
                }
            }

            model.ObligatedCredits = model.ObligatedCredits ?? 0m;
            model.ElectiveCredits = model.ElectiveCredits ?? 0m;
            model.TotalOfRequiredCredits = (model.ObligatedCredits ?? 0m) + (model.ElectiveCredits ?? 0m);
            model.CreatedAt = DateTime.Now;
            model.IsDeleted = false;

            var added = _db.AddAcademicProgram(model);
            if (!added)
            {
                return StatusCode(500, new { message = "Failed to add academic program. Please try again." });
            }

            return Ok(new
            {
                message = "Academic program added successfully.",
                program = new
                {
                    model.AcademicProgramId,
                    model.AcademicProgramName,
                    model.MajorId,
                    model.FacultyId,
                    model.Language,
                    model.Description,
                    model.NumberOfSemester,
                    model.ObligatedCredits,
                    model.ElectiveCredits,
                    model.TotalOfRequiredCredits
                }
            });
        }
    }
}
