using SIMS.Models;

namespace SIMS.ViewModels
{
    public class StudentDetailsViewModel
    {
        public List<Student> Students { get; set; }
        public List<AcademicProgram> Programs { get; set; }

        public StudentDetailsViewModel() { }
    }
}
