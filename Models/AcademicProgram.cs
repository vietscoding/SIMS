namespace SIMS.Models
{
    public class AcademicProgram
    {
        public string AcademicProgramName { get; set; } = string.Empty;
        public int AcademicProgramId { get; set; } = 0;
        public int MajorId { get; set; } = default;
        public int FacultyId { get; set; } = default;
        public string Language { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int NumberOfSemester { get; set; } = default;
        public int TotalOfRequiredCredits { get; set; } = default;
        public int ObligatedCredits { get; set; } = default;
        public int ElectiveCredits { get; set; } = default;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        public AcademicProgram() { }

    }
}
