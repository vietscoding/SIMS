using SIMS.Models;

namespace SIMS.ViewModels
{
    public class AcademicProgramDetailViewModel
    {
        public AcademicProgram AcademicProgram { get; set; } = new AcademicProgram();

        public Curriculum? Curriculum { get; set; } = null;
        public Major? Major { get; set; } = null;
        public Faculty? Faculty { get; set; } = null;
        public CourseDependency? CourseDependency { get; set; } = null;

        public List<Curriculum>? Curriculums { get; set; } = new();
        public List<Major>? Majors { get; set; } = new();
        public List<Faculty>? Faculties { get; set; } = new();
        public List<CourseDependency>? CourseDependencies { get; set; } = new();
        public AcademicProgramDetailViewModel() { }
    }
}
