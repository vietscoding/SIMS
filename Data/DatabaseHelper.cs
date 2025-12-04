// DatabaseHelper.cs

using Microsoft.EntityFrameworkCore;
using SIMS.Models;
//using System.Data.Entity;
using System.Diagnostics.Contracts;


namespace SIMS.Data
{
    public class DatabaseHelper
    {
        private readonly AppDbContext _context;

        public DatabaseHelper(AppDbContext context)
        {
            _context = context;
        }

        public List<AcademicProgram> GetAllAcademicPrograms() // Lấy tất cả các chương trình học
        {
            return _context.AcademicPrograms
                .Include(p => p.Major)
                .Include(p => p.Faculty)
                .AsNoTracking()
                .OrderBy(p => p.AcademicProgramName ?? "")
                .ToList();
        }

        // New: expose IQueryable so controllers can build server-side filtered queries
        public IQueryable<AcademicProgram> GetAcademicPrograms()
        {
            // Do not AsNoTracking here so callers can choose tracking or not; include navigations so projections can use names.
            return _context.AcademicPrograms
                .Include(p => p.Major)
                .Include(p => p.Faculty)
                .AsQueryable();
        }


        
        public List<Student> GetAllStudents() // Lấy tất cả các sinh viên
        {
            return _context.Students
                .Include(s => s.Person)
                .AsNoTracking()
                .OrderBy(s => s.StudentId)
                .ToList();
        }

        public List<Course> GetAllCourses() // Lấy tất cả các học phần
        {
            return _context.Courses
                .AsNoTracking()
                .OrderBy(c => c.CourseName ?? "")
                .ToList();
        }

        public List<Faculty> GetAllFaculties() // Lấy tất cả các khoa
        {
            return _context.Faculties
                .AsNoTracking()
                .OrderBy(f => f.FacultyName ?? "")
                .ToList();
        }

        public List<Major> GetAllMajors() // Lấy tất cả các ngành
        {
            // Project into an anonymous type first so EF Core emits COALESCE for nullable string columns.
            // This avoids ADO.NET calling GetString on NULL DB values.
            var rows = _context.Majors
                .AsNoTracking()
                .Select(m => new
                {
                    m.MajorId,
                    MajorName = m.MajorName ?? string.Empty,
                    AlternativeMajorName = m.AlternativeMajorName ?? string.Empty,
                    MajorCode = m.MajorCode ?? string.Empty,
                    TenNganh = m.TenNganh ?? string.Empty,
                    m.FacultyId,
                    m.CreatedAt,
                    m.UpdatedAt,
                    m.IsDeleted
                })
                .OrderBy(x => x.MajorName)
                .ToList();

            // Materialize plain Major instances in-memory from the projected rows.
            var result = rows.Select(r => new Major
            {
                MajorId = r.MajorId,
                MajorName = r.MajorName,
                AlternativeMajorName = r.AlternativeMajorName,
                MajorCode = r.MajorCode,
                TenNganh = r.TenNganh,
                FacultyId = r.FacultyId,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                IsDeleted = r.IsDeleted
            }).ToList();

            return result;
        }

        //public List<Person> GetAllPeople() // Lấy tất cả các người
        //{
        //    return _context.People
        //        .AsNoTracking()
        //        .OrderBy(p => p.PersonId)
        //        .ToList();
        //}

        public IQueryable<Person> GetPeople()
        {
            return _context.People;
        }

        public List<Person> GetAllPeople()
        {
            var rows = _context.People
                .AsNoTracking()
                .Select(p => new
                {
                    p.PersonId,
                    CitizenIdNumber = p.CitizenIdNumber ?? string.Empty,
                    FullName = p.FullName ?? string.Empty,
                    Gender = p.Gender ?? null,
                    DateOfBirth = p.DateOfBirth ?? null,
                    Email = p.Email ?? null,
                    PhoneNumber = p.PhoneNumber ?? null,
                    Address = p.Address ?? null,
                    Nationality = p.Nationality ?? null,
                    p.CreatedAt,
                    p.UpdatedAt,
                    p.IsDeleted
                })
                .OrderBy(x => x.FullName)
                .ToList();

            var result = rows.Select(r => new Person
            {
                PersonId = r.PersonId,
                CitizenIdNumber = r.CitizenIdNumber,
                FullName = r.FullName,
                Gender = r.Gender,
                DateOfBirth = r.DateOfBirth,
                Email = r.Email,
                PhoneNumber = r.PhoneNumber,
                Address = r.Address,
                Nationality = r.Nationality,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                IsDeleted = r.IsDeleted
            }).ToList();

            return result;
        }

        public List<AcademicProgram> GetProgramsByMajorId(int majorId) // Lấy các chương trình học theo ID ngành
        {
            return _context.AcademicPrograms
                .AsNoTracking()
                .Where(p => p.MajorId == majorId)
                .OrderBy(p => p.AcademicProgramName ?? "")
                .ToList();
        }

        public List<AcademicProgram> GetProgramsByFacultyId(int facultyId) // Lấy các chương trình học theo ID khoa
        {
            return _context.AcademicPrograms
                .AsNoTracking()
                .Where(p => p.FacultyId == facultyId)
                .OrderBy(p => p.AcademicProgramName ?? "")
                .ToList();
        }

        public AcademicProgram GetAcademicProgramById(int programId) // Lấy chương trình học theo ID
        {
            return _context.AcademicPrograms
                .Include(f => f.Faculty)
                .Include(m => m.Major)
                .AsNoTracking()
                .FirstOrDefault(p => p.AcademicProgramId == programId) ?? new AcademicProgram();  
        }

        public Student GetStudentById(int studentId) // Lấy sinh viên theo ID
        {
            return _context.Students
                .Include(s => s.Person)
                .AsNoTracking()
                .FirstOrDefault(s => s.StudentId == studentId) ?? new Student();
        }

        public Person GetPersonById(int personId) // Lấy người theo ID
        {
            return _context.People
                .AsNoTracking()
                .FirstOrDefault(p => p.PersonId == personId) ?? new Person();
        }

        public Faculty GetFacultyById(int facultyId) // Lấy khoa theo ID
        {
            return _context.Faculties
                .AsNoTracking()
                .FirstOrDefault(f => f.FacultyId == facultyId) ?? new Faculty();
        }

        public List<Faculty> GetAllDistinctFaculties()
        {
            return _context.Faculties
                .GroupBy(f => f.FacultyName)
                .Select(g => g.First())
                .ToList();
        }

        public Major GetMajorById(int majorId) // Lấy ngành theo ID
        {
            return _context.Majors
                .AsNoTracking()
                .FirstOrDefault(m => m.MajorId == majorId) ?? new Major();
        }

        public bool AddPerson(Person person) // Thêm người mới
        {
            _context.People.Add(person);
            return _context.SaveChanges() > 0;
        }

        public bool AddStudent(Student student) // Thêm sinh viên mới
        {
            _context.Students.Add(student);
            return _context.SaveChanges() > 0;
        }

        public bool AddCourse(Course course) // Thêm học phần mới
        {
            _context.Courses.Add(course);
            return _context.SaveChanges() > 0;
        }

        public bool AddAcademicProgram(AcademicProgram program) // Thêm chương trình học mới
        {
            _context.AcademicPrograms.Add(program);
            return _context.SaveChanges() > 0;
        }

        public bool AddFaculty(Faculty faculty) // Thêm khoa mới
        {
            _context.Faculties.Add(faculty);
            return _context.SaveChanges() > 0;
        }

        public bool AddMajor(Major major) // Thêm ngành mới
        {
            _context.Majors.Add(major);
            return _context.SaveChanges() > 0;
        }

        // Add this method into the existing DatabaseHelper class.
        public bool AddCurriculum(Curriculum curriculum)
        {
            if (curriculum == null) return false;
            _context.Curriculum.Add(curriculum);
            return _context.SaveChanges() > 0;
        }

        public bool RemoveStudent(int studentId) // Xóa sinh viên theo ID
        {
            var student = _context.Students.Find(studentId);
            if (student == null)
            {
                return false;
            }
            _context.Students.Remove(student);
            return _context.SaveChanges() > 0;
        }

        public bool RemovePerson(int personId) // Xóa người theo ID
        {
            var person = _context.People.Find(personId);
            if (person == null)
            {
                return false;
            }
            _context.People.Remove(person);
            return _context.SaveChanges() > 0;
        }

        public bool RemoveCourse(int courseId) // Xóa học phần theo ID
        {
            var course = _context.Courses.Find(courseId);
            if (course == null)
            {
                return false;
            }
            _context.Courses.Remove(course);
            return _context.SaveChanges() > 0;
        }   


        public bool RemoveAcademicProgram(int programId) // Xóa chương trình học theo ID
        {
            var program = _context.AcademicPrograms.Find(programId);
            if (program == null)
            {
                return false;
            }
            _context.AcademicPrograms.Remove(program);
            return _context.SaveChanges() > 0;
        }

        public bool RemoveFaculty(int facultyId) // Xóa khoa theo ID
        {
            var faculty = _context.Faculties.Find(facultyId);
            if (faculty == null)
            {
                return false;
            }
            _context.Faculties.Remove(faculty);
            return _context.SaveChanges() > 0;
        }

        public bool RemoveMajor(int majorId) // Xóa ngành mới
        {
            var major = _context.Majors.Find(majorId);
            if (major == null)
            {
                return false;
            }
            _context.Majors.Remove(major);
            return _context.SaveChanges() > 0;
        }

        public bool UpdatePerson(Person person) // Cập nhật thông tin người
        {
            var existingPerson = _context.People.Find(person.PersonId);
            if (existingPerson == null)
            {
                return false;
            }
            existingPerson.FullName = person.FullName;
            existingPerson.CitizenIdNumber = person.CitizenIdNumber;
            existingPerson.Gender = person.Gender;
            existingPerson.DateOfBirth = person.DateOfBirth;
            existingPerson.Email = person.Email;
            existingPerson.PhoneNumber = person.PhoneNumber;
            existingPerson.Address = person.Address;
            existingPerson.Nationality = person.Nationality;

            _context.SaveChanges();
            return true;
        }

        public bool UpdateCourse(Course course) // Cập nhật thông tin học phần
        {
            var existingCourse = _context.Courses.Find(course.CourseId);
            if (existingCourse == null)
            {
                return false;
            }
            existingCourse.CourseName = course.CourseName;
            existingCourse.CourseCode = course.CourseCode;
            existingCourse.TenHocPhan = course.TenHocPhan;
            existingCourse.FacultyId = course.FacultyId;
            existingCourse.LectureCredits = course.LectureCredits;
            existingCourse.PracticalCredits = course.PracticalCredits;
            existingCourse.InternshipCredits = course.InternshipCredits;
            existingCourse.CapstoneCredits = course.CapstoneCredits;
            existingCourse.CourseSummary = course.CourseSummary;


            _context.SaveChanges();
            return true;
        }

        // Updated: update full academic program fields
        public bool UpdateAcademicProgram(AcademicProgram program) // Cập nhật thông tin chương trình học
        {
            var existingProgram = _context.AcademicPrograms.Find(program.AcademicProgramId);
            if (existingProgram == null)
            {
                return false;
            }

            existingProgram.AcademicProgramName = program.AcademicProgramName;
            existingProgram.MajorId = program.MajorId;
            existingProgram.FacultyId = program.FacultyId;
            existingProgram.Language = program.Language;
            existingProgram.Description = program.Description;
            existingProgram.NumberOfSemester = program.NumberOfSemester;
            existingProgram.ObligatedCredits = program.ObligatedCredits;
            existingProgram.ElectiveCredits = program.ElectiveCredits;
            existingProgram.TotalOfRequiredCredits = (program.ObligatedCredits ?? 0m) + (program.ElectiveCredits ?? 0m);
            existingProgram.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
            return true;
        }

        public bool UpdateFaculty(Faculty faculty) // Cập nhật thông tin khoa
        {
            var existingFaculty = _context.Faculties.Find(faculty.FacultyId);
            if (existingFaculty == null)
            {
                return false;
            }
            existingFaculty.FacultyName = faculty.FacultyName;
            
            _context.SaveChanges();
            return true;
        }

        public bool UpdateMajor(Major major) // Cập nhật thông tin ngành
        {
            var existingMajor = _context.Majors.Find(major.MajorId);
            if (existingMajor == null)
            {
                return false;
            }
            existingMajor.MajorName = major.MajorName;
            
            _context.SaveChanges();
            return true;
        }

        


        public bool CanConnect()
        {
            return _context.Database.CanConnect();
        }

        public bool UpdateStudent(Student student) // Cập nhật thông tin sinh viên
        {
            var existingStudent = _context.Students
                .Include(s => s.Person)
                .FirstOrDefault(s => s.StudentId == student.StudentId);

            if (existingStudent == null || existingStudent.Person == null)
            {
                return false;
            }

            existingStudent.Person.PhoneNumber = student.PhoneNumber;
            existingStudent.Person.Address = student.Address;
            existingStudent.Person.UpdatedAt = DateTime.Now;

            _context.Entry(existingStudent.Person).State = EntityState.Modified;
            _context.SaveChanges();

            return true;
        }


        public IQueryable<Course> GetCourses()
        {
            // Ensure Faculty navigation is available to callers so views / APIs can use Faculty.FacultyName
            return _context.Courses.Include(c => c.Faculty);
        }

        public Course GetCourseById(int courseId)
        {
            return _context.Courses
                .AsNoTracking()
                .Include(c => c.Faculty)
                .FirstOrDefault(c => c.CourseId == courseId) ?? new Course();
        }

        // Lấy tất cả các giáo trình / khung chương trình (Curriculum)
        public List<Curriculum> GetAllCurriculums()
        {
            return _context.Curriculum
                .Include(c => c.Program)
                .Include(c => c.Course)
                .AsNoTracking()
                .OrderBy(c => c.CurriculumId)
                .ToList();
        }

        // Lấy curriculum theo ID
        public Curriculum GetCurriculumById(int curriculumId)
        {
            return _context.Curriculum
                .Include(c => c.Program)
                .Include(c => c.Course)
                .AsNoTracking()
                .FirstOrDefault(c => c.CurriculumId == curriculumId) ?? new Curriculum();
        }

        // Lấy các curriculum theo ProgramId
        /// <summary>
        /// Lấy tất cả các chương trình học theo một ProgramId cụ thể
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public List<Curriculum> GetAllCurriculumsByProgramId(int programId)
        {
            return _context.Curriculum
                .Include(c => c.Course)
                .AsNoTracking()
                .Where(c => c.ProgramId == programId)
                .OrderBy(c => c.CurriculumId)
                .ToList();
        }

        // Lấy tất cả các CourseDependency
        public List<CourseDependency> GetAllCourseDependencies()
        {
            return _context.CourseDependencyies
                .Include(cd => cd.Curriculum)
                .Include(cd => cd.PreviousCourse)
                .Include(cd => cd.CorequisiteCourse)
                .Include(cd => cd.PrerequisiteCourse)
                .AsNoTracking()
                .OrderBy(cd => cd.CourseDependencyId)
                .ToList();
        }

        // Lấy tất cả CourseDependency theo CurriculumId
        public List<CourseDependency> GetAllCourseDependenciesByCurriculumId(int curriculumId)
        {
            return _context.CourseDependencyies
                .Include(cd => cd.PreviousCourse)
                .Include(cd => cd.CorequisiteCourse)
                .Include(cd => cd.PrerequisiteCourse)
                .AsNoTracking()
                .Where(cd => cd.CurriculumId == curriculumId)
                .OrderBy(cd => cd.CourseDependencyId)
                .ToList();
        }

        // Lấy CourseDependency theo ID
        public CourseDependency GetCourseDependencyById(int courseDependencyId)
        {
            return _context.CourseDependencyies
                .Include(cd => cd.Curriculum)
                .Include(cd => cd.PreviousCourse)
                .Include(cd => cd.CorequisiteCourse)
                .Include(cd => cd.PrerequisiteCourse)
                .AsNoTracking()
                .FirstOrDefault(cd => cd.CourseDependencyId == courseDependencyId) ?? new CourseDependency();
        }

        // Add this method inside the existing DatabaseHelper class.
        //public bool RemoveCurriculum(int curriculumId)
        //{
        //    // Guard
        //    if (curriculumId <= 0) return false;

        //    // Try find the curriculum
        //    var curriculum = _context.Curriculum.Find(curriculumId);
        //    if (curriculum == null) return false;

        //    // Prefer soft-delete if the column exists; fallback to physical delete.
        //    try
        //    {
        //        // If a soft-delete column is present, mark it and update timestamp
        //        if (curriculum.GetType().GetProperty("IsDeleted") != null)
        //        {
        //            curriculum.IsDeleted = true;
        //            curriculum.UpdatedAt = DateTime.Now;
        //            _context.Entry(curriculum).State = EntityState.Modified;
        //        }
        //        else
        //        {
        //            _context.Curriculum.Remove(curriculum);
        //        }

        //        return _context.SaveChanges() > 0;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    _context.Curriculum.Remove(curriculum);
        //    return _context.SaveChanges() > 0;


        //}

        public bool RemoveCurriculum(int curriculumId)
        {
            // Guard
            if (curriculumId <= 0) return false;

            // Try find the curriculum
            var curriculum = _context.Curriculum.Find(curriculumId);

            // Nếu không tìm thấy, coi như đã 'xóa' (hoặc không cần làm gì)
            if (curriculum == null) return false;

            try
            {
                // ➡️ THỰC HIỆN XÓA CỨNG: Loại bỏ đối tượng khỏi Context
                // Điều này sẽ tạo ra lệnh DELETE trong cơ sở dữ liệu
                _context.Curriculum.Remove(curriculum);

                // Lưu thay đổi vào cơ sở dữ liệu
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                // Ghi log (nên thêm logic ghi log thực tế ở đây)
                // Ví dụ: Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
