namespace SIMS.Models
{
    public class Student : Person
    {

        public string StudentId { get; set; } = string.Empty;
        public string PhoneNumberOfRelatives { get; set; } = string.Empty; // số đt của người thân
        
        /// <summary>
        /// Phân loại sinh viên dựa trên năm nhập học
        /// Năm nhất: Fresher
        /// Năm 2: Sophomore
        /// Năm 3: Junior
        /// Năm 4: Senior
        /// </summary>
        public string StudentClassificationByYear { get; set; } = string.Empty;

        public Student(string id, string name, DateTime dateOfBirth)
        {
            StudentId = id;
            FullName = name;
            DateOfBirth = dateOfBirth;

        }

        public IEnumerable<Student> Students { get; private set; }
    }
}
