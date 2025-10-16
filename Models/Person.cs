namespace SIMS.Models
{
    public class Person
    {
        public string PersonId { get; set; } = string.Empty;

        public string FullName { get; set; } = "This is a name";

        public string Email { get; set; } = string.Empty;

        public string CitizenIdNumber { get; set; } = string.Empty;

        public bool? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string PhoneNumber { get; set; } = string.Empty; 

        public string Address { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public Person() { }



    }

    

}
