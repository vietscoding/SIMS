using Microsoft.AspNetCore.Mvc;
using SIMS.Data;
using SIMS.ViewModels;
using SIMS.Models;
using System.Linq;
using System;

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
        public IActionResult PeopleList()
        {
            var people = _db.GetAllPeople();
            return View("PeopleList", people);
        }
        public IActionResult AcademicProgram()
        {
            var programs = _db.GetAllAcademicPrograms();
            return View("AcademicProgram", programs);
        }
        public IActionResult Course(int page = 1)
        {
            int pageSize = 10;
            var query = _db.GetCourses(); // IQueryable
            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var data = query
                .OrderBy(c => c.CourseId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Faculties = _db.GetAllDistinctFaculties();
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;

            return View(data);
        }

        public IActionResult CourseDetails(int id)
        {
            var course = _db.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
            return View("Course", course);
        }

        [HttpGet]
        public IActionResult GetCourseDetails(int id) // Trả về kiểu JSON cho chi tiết học phần được gọi bằng AJAX trong trang Course.cshtml
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid course ID." });
            }
            var course = _db.GetCourseById(id);
            if (course == null || course.CourseId == 0)
            {
                return NotFound(new { message = "Course not found." });
            }
            return Json(new
            {
                courseId = course.CourseId,
                courseCode = course.CourseCode,
                courseName = course.CourseName,
                facultyId = course.FacultyId,
                totalCredits = course.TotalCredits,
                lectureCredits = course.LectureCredits,
                practicalCredits = course.PracticalCredits,
                internshipCredits = course.InternshipCredits,
                capstoneCredits = course.CapstoneCredits,
                summary = course.CourseSummary
            });
        }

        // Added server-side filtering support: name, code, type, programId, semester.
        // name -> matches CourseName or TenHocPhan
        // code -> matches CourseCode
        // type/programId/semester: these properties are not present directly on Course in the current model,
        // so type will attempt a best-effort match against CourseSummary/CourseName and programId/semester are accepted
        // but currently ignored (no related navigation present). They are kept for API compatibility and future extension.
        [HttpGet]
        public IActionResult GetCourses(int page = 1, string? name = null, string? code = null, string? type = null, int? programId = null, int? semester = null)
        {
            int pageSize = 10;
            var query = _db.GetCourses(); // IQueryable<Course>

            // Exclude deleted records if applicable
            query = query.Where(c => !c.IsDeleted);

            // Apply filters
            if (!string.IsNullOrWhiteSpace(name))
            {
                var term = name.Trim();
                query = query.Where(c =>
                    ((c.CourseName ?? string.Empty).Contains(term)) ||
                    ((c.TenHocPhan ?? string.Empty).Contains(term))
                );
            }

            if (!string.IsNullOrWhiteSpace(code))
            {
                var term = code.Trim();
                query = query.Where(c => (c.CourseCode ?? string.Empty).Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                // Course doesn't have a dedicated "type" property in the current model.
                // Best-effort: match against CourseSummary and CourseName.
                var term = type.Trim();
                query = query.Where(c =>
                    ((c.CourseSummary ?? string.Empty).Contains(term)) ||
                    ((c.CourseName ?? string.Empty).Contains(term))
                );
            }

            // programId and semester are accepted but not applied because Course model has no ProgramId/Semester columns.
            // Keep parameters to preserve client contract and for future extension.
            // Example hooks for future implementation:
            // if (programId.HasValue) { /* join to related table and filter */ }
            // if (semester.HasValue) { /* join to related offerings and filter */ }

            // Calculate totals after filtering
            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var data = query
                .OrderBy(c => c.CourseId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Json(new
            {
                courses = data.Select(c => new
                {
                    courseId = c.CourseId,
                    courseCode = c.CourseCode,
                    courseName = c.CourseName,
                    facultyId = c.FacultyId,
                    totalCredits = c.TotalCredits,
                    lectureCredits = c.LectureCredits,
                    practicalCredits = c.PracticalCredits,
                    internshipCredits = c.InternshipCredits,
                    capstoneCredits = c.CapstoneCredits,
                    courseSummary = c.CourseSummary
                }),
                currentPage = page,
                totalPages = totalPages,
                totalItems = totalItems,
                appliedFilters = new
                {
                    name = name,
                    code = code,
                    type = type,
                    programId = programId,
                    semester = semester
                }
            });
        }

        // New endpoint to create a course via AJAX (expects JSON body)
        [HttpPost]
        public IActionResult AddCourse([FromBody] CourseCreateModel model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid request payload." });
            }

            // Basic server-side validation
            if (string.IsNullOrWhiteSpace(model.CourseName))
            {
                return BadRequest(new { message = "Course name is required." });
            }
            if (string.IsNullOrWhiteSpace(model.CourseCode))
            {
                return BadRequest(new { message = "Course code is required." });
            }

            // Validate FacultyId
            if (model.FacultyId == null)
            {
                return BadRequest(new { message = "Faculty in charge is required." });
            }
            else
            {
                var list = _db.GetAllDistinctFaculties();
                if (!list.Any(f => f.FacultyId == model.FacultyId))
                {
                    return BadRequest(new { message = "Specified faculty does not exist." });
                }
            }

            var codeTrim = model.CourseCode.Trim();

            // Duplicate check by CourseCode
            var exists = _db.GetCourses().Any(c => (c.CourseCode ?? string.Empty) == codeTrim);
            if (exists)
            {
                return Conflict(new { message = $"A course with code '{codeTrim}' already exists." });
            }

            // Map to entity
            var course = new Course
            {
                CourseName = model.CourseName?.Trim(),
                TenHocPhan = model.TenHocPhan?.Trim(),
                CourseCode = codeTrim,
                FacultyId = model.FacultyId,
                LectureCredits = model.LectureCredits,
                PracticalCredits = model.PracticalCredits,
                InternshipCredits = model.InternshipCredits,
                CapstoneCredits = model.CapstoneCredits,
                CourseSummary = model.CourseSummary?.Trim(),
                IsDeleted = false
            };

            // Compute TotalCredits if not provided
            //decimal total = 0m;
            //total += course.LectureCredits ?? 0m;
            //total += course.PracticalCredits ?? 0m;
            //total += course.InternshipCredits ?? 0m;
            //total += course.CapstoneCredits ?? 0m;
            //course.TotalCredits = total;

            var added = _db.AddCourse(course);
            if (added == false)
            {
                return StatusCode(500, new { message = "Failed to add course. Please try again." });
            }

            return Ok(new
            {
                message = "Course added successfully.",
                course = new
                {
                    course.CourseId,
                    course.CourseCode,
                    course.CourseName,
                    course.FacultyId,
                    course.LectureCredits,
                    course.PracticalCredits,
                    course.InternshipCredits,
                    course.CapstoneCredits,
                    course.CourseSummary
                    //course.TotalCredits // The database will compute this automatically
                }
            });
        }

        

        
    }
}