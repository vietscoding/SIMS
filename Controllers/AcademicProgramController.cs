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
            return View(academicProgramList);
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

        [HttpPost]
        public IActionResult DeleteAcademicProgram([FromBody] int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid program id." });
            }

            var existing = _db.GetAcademicProgramById(id);
            if (existing == null || existing.AcademicProgramId == 0)
            {
                return NotFound(new { success = false, message = "Program not found." });
            }

            var removed = _db.RemoveAcademicProgram(id);
            if (!removed)
            {
                return StatusCode(500, new { success = false, message = "Failed to delete program. It may be referenced by other data." });
            }

            return Ok(new { success = true, message = "Program deleted successfully." });
        }

        [HttpPost]
        public IActionResult UpdateAcademicProgram([FromBody] AcademicProgram model)
        {
            if (model == null)
            {
                return BadRequest(new { success = false, message = "Invalid request payload." });
            }

            if (model.AcademicProgramId <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid program id." });
            }

            if (string.IsNullOrWhiteSpace(model.AcademicProgramName))
            {
                return BadRequest(new { success = false, message = "Program name is required." });
            }

            if (model.NumberOfSemester.HasValue && model.NumberOfSemester <= 0)
            {
                return BadRequest(new { success = false, message = "Number of semesters must be a positive integer." });
            }

            // Validate referenced entities if provided
            if (model.FacultyId.HasValue)
            {
                var fac = _db.GetFacultyById(model.FacultyId.Value);
                if (fac == null || fac.FacultyId == 0)
                {
                    return BadRequest(new { success = false, message = "Specified faculty does not exist." });
                }
            }

            if (model.MajorId.HasValue)
            {
                var maj = _db.GetMajorById(model.MajorId.Value);
                if (maj == null || maj.MajorId == 0)
                {
                    return BadRequest(new { success = false, message = "Specified major does not exist." });
                }
            }

            // Ensure numeric credit fields have sane defaults
            model.ObligatedCredits = model.ObligatedCredits ?? 0m;
            model.ElectiveCredits = model.ElectiveCredits ?? 0m;
            model.TotalOfRequiredCredits = (model.ObligatedCredits ?? 0m) + (model.ElectiveCredits ?? 0m);
            model.UpdatedAt = DateTime.Now;

            var existing = _db.GetAcademicProgramById(model.AcademicProgramId);
            if (existing == null || existing.AcademicProgramId == 0)
            {
                return NotFound(new { success = false, message = "Program not found." });
            }

            var updated = _db.UpdateAcademicProgram(model);
            if (!updated)
            {
                return StatusCode(500, new { success = false, message = "Failed to update program." });
            }

            return Ok(new
            {
                success = true,
                message = "Program updated successfully.",
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

        [HttpGet]
        public IActionResult GetPrograms(
            int page = 1,
            string? name = null,
            int? majorId = null,
            int? facultyId = null,
            string? language = null,
            int? numberOfSemester = null,
            decimal? obligatedMin = null, decimal? obligatedMax = null,
            decimal? electiveMin = null, decimal? electiveMax = null,
            decimal? totalMin = null, decimal? totalMax = null,
            string? description = null)
        {
            int pageSize = 10;
            var query = _db.GetAcademicPrograms().AsQueryable();

            // exclude deleted
            //query = query.Where(p => !p.IsDeleted);

            if (!string.IsNullOrWhiteSpace(name))
            {
                var term = name.Trim();
                query = query.Where(p =>
                    (p.AcademicProgramName ?? string.Empty).Contains(term) ||
                    (p.Description ?? string.Empty).Contains(term)
                );
            }

            if (majorId.HasValue)
            {
                query = query.Where(p => p.MajorId == majorId.Value);
            }

            if (facultyId.HasValue)
            {
                query = query.Where(p => p.FacultyId == facultyId.Value);
            }

            if (!string.IsNullOrWhiteSpace(language))
            {
                var term = language.Trim();
                query = query.Where(p => (p.Language ?? string.Empty).Contains(term));
            }

            if (numberOfSemester.HasValue)
            {
                query = query.Where(p => p.NumberOfSemester == numberOfSemester.Value);
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                var term = description.Trim();
                query = query.Where(p =>
                    (p.Description ?? string.Empty).Contains(term) ||
                    (p.AcademicProgramName ?? string.Empty).Contains(term)
                );
            }

            if (obligatedMin.HasValue)
            {
                query = query.Where(p => (p.ObligatedCredits ?? 0m) >= obligatedMin.Value);
            }
            if (obligatedMax.HasValue)
            {
                query = query.Where(p => (p.ObligatedCredits ?? 0m) <= obligatedMax.Value);
            }

            if (electiveMin.HasValue)
            {
                query = query.Where(p => (p.ElectiveCredits ?? 0m) >= electiveMin.Value);
            }
            if (electiveMax.HasValue)
            {
                query = query.Where(p => (p.ElectiveCredits ?? 0m) <= electiveMax.Value);
            }

            // total from obligated + elective (computed column may also exist)
            if (totalMin.HasValue)
            {
                query = query.Where(p => ((p.ObligatedCredits ?? 0m) + (p.ElectiveCredits ?? 0m)) >= totalMin.Value);
            }
            if (totalMax.HasValue)
            {
                query = query.Where(p => ((p.ObligatedCredits ?? 0m) + (p.ElectiveCredits ?? 0m)) <= totalMax.Value);
            }

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var data = query
                .OrderBy(p => p.AcademicProgramId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToList();

            return Json(new
            {
                programs = data.Select(p => new
                {
                    academicProgramId = p.AcademicProgramId,
                    academicProgramName = p.AcademicProgramName,
                    majorId = p.MajorId,
                    majorName = p.Major?.MajorName,
                    facultyId = p.FacultyId,
                    facultyName = p.Faculty?.FacultyName,
                    language = p.Language,
                    description = p.Description,
                    numberOfSemester = p.NumberOfSemester,
                    obligatedCredits = p.ObligatedCredits,
                    electiveCredits = p.ElectiveCredits,
                    totalCredits = ((p.ObligatedCredits ?? 0m) + (p.ElectiveCredits ?? 0m))
                }),
                currentPage = page,
                totalPages = totalPages,
                totalItems = totalItems
            });
        }
    }
}
