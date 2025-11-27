using SIMS.Models;

namespace SIMS.ViewModels
{
    public class AcademicProgramDetailViewModel
    {
        public AcademicProgram AcademicProgram { get; set; } = new AcademicProgram();
        public List<Curriculum> Curriculums { get; set; } = new();

        public List<Major> Majors { get; set; } = new();
        public List<Faculty> Faculties { get; set; } = new();
        public List<CourseDependency> CourseDependencies { get; set; } = new();
        public AcademicProgramDetailViewModel() { }
    }
}
