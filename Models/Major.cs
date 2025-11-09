namespace SIMS.Models
{
    public class Major
    {
        public int MajorId { get; set; }
        public string MajorName { get; set; }
        public string AlternativeMajorName { get; set; }
        public string TenNganh { get; set; }
        public int FacultyId { get; set; }
        public Major() { }
    }
}
