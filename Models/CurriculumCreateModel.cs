namespace SIMS.Models
{
    // DTO for adding a Curriculum (course in a program)
    public class CurriculumCreateModel
    {
        public int ProgramId { get; set; }
        public int CourseId { get; set; }
        // Optional flags can be added later (IsElective, etc.)
    }
}