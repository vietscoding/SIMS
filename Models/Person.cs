namespace SIMS.Models
{
    public class Person
    {
        public string personId { get; set; } = string.Empty;

        public string FullName { get; set; } = "This is a name";

        public string Email { get; set; } = string.Empty;

        public string CitizenIdNumber { get; set; } = string.Empty;

        public bool? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? ethnicity { get; set; } // dân tộc, vd: H'Mông, Ê-đê, Kinh,...

        public string PhoneNumber { get; set; } = string.Empty; // sđt của sinh viên

        public DateTime Created { get; set; }


        public DateTime Updated { get; set; }

        private string? Password { get; set; }

    }
}
