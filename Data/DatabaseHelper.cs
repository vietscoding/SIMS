using Microsoft.EntityFrameworkCore;
using SIMS.Models;
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
                .AsNoTracking()
                .OrderBy(p => p.AcademicProgramName ?? "")
                .ToList();
        }

        public List<Person> GetAllPeople() // Lấy tất cả các người
        {
            return _context.People
                .AsNoTracking()
                .OrderBy(p => p.PersonId)
                .ToList();
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
            return _context.Majors
                .AsNoTracking()
                .OrderBy(m => m.MajorName ?? "")
                .ToList();
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

        public bool RemoveMajor(int majorId) // Xóa ngành theo ID
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
            existingPerson.DateOfBirth = person.DateOfBirth;
            
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
            
            _context.SaveChanges();
            return true;
        }

        public bool UpdateAcademicProgram(AcademicProgram program) // Cập nhật thông tin chương trình học
        {
            var existingProgram = _context.AcademicPrograms.Find(program.AcademicProgramId);
            if (existingProgram == null)
            {
                return false;
            }
            existingProgram.AcademicProgramName = program.AcademicProgramName;
            
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
            existingStudent.Person.Updated = DateTime.Now;

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

    }
}
