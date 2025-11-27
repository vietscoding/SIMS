using SIMS.Models;

namespace SIMS.ViewModels
{
    public class AcademicProgramViewModel
    {
        public List<AcademicProgram>? AcademicPrograms { get; set; } = new();

        public Major? Major { get; set; } = new();

        public Faculty? Faculty { get; set; } = new();

        public List<Major>? Majors { get; set; } = new();
        public List<Faculty>? Faculties { get; set; } = new();
        public AcademicProgramViewModel() { }
        
    }
}
