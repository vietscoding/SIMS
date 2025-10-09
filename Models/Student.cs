namespace SIMS.Models
{
    public class Student : Person
    {
        public string StudentId { get; set; } = string.Empty;
        public string PhoneNumberOfRelatives { get; set; } = string.Empty; // số đt của người thân

        public Student(string id, string name, DateTime dateOfBirth)
        {
            StudentId = id;
            FullName = name;
            DateOfBirth = dateOfBirth;

        }

        public IEnumerable<Student> Students { get; private set; }


    }
}
