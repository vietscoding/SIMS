using SIMS.Models;

namespace SIMS.ViewModels
{
    public class StudentDetailsViewModel
    {
        public List<Student> Students { get; set; } = new();
        public List<AcademicProgram> Programs { get; set; } = new();

        public StudentDetailsViewModel() { }
    }
}
