using SIMS.Models;

namespace SIMS.ViewModels
{
    public class AcademicProgramDetailViewModel
    {
        public AcademicProgram AcademicProgram { get; set; } = new AcademicProgram();
        public List<Curriculum> Curriculums { get; set; } = new();

        public List<CourseDependency> CourseDependencies { get; set; } = new();
        public AcademicProgramDetailViewModel() { }
    }
}
