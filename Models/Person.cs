using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace SIMS.Models
{
    [Table("Person")]
    public class Person
    {
        [Key]
        [Column("person_id")]
        public int PersonId { get; set; }

        [Column("citizen_id_number")]
        public string CitizenIdNumber { get; set; } = string.Empty;

        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;

        [Column("gender")]
        public bool? Gender { get; set; }

        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [Column("email")]
        public string? Email { get; set; } = string.Empty;

        [Column("phone_number")]
        public string? PhoneNumber { get; set; } = string.Empty;

        [Column("address")]
        public string? Address { get; set; } = string.Empty;

        [Column("nationality")]
        public string? Nationality { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("is_deleted")]
        public bool? IsDeleted { get; set; }

        [JsonIgnore]
        public virtual Student? Student { get; set; }

        public Person() { }

        public bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return false;
            }

            return Regex.IsMatch(phoneNumber, @"^0\d{9}$");
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
        }

        public bool IsValidFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return false;
            }

            return Regex.IsMatch(fullName.Trim(), @"^[A-Za-zÀ-ỹ\s'\-\.]+$");
        }

        public bool IsValidGender(bool? gender)
        {
            return gender.HasValue;
        }

        public bool IsValidDateOfBirth(DateTime? dateOfBirth)
        {
            if (!dateOfBirth.HasValue)
            {
                return false;
            }

            var dob = dateOfBirth.Value.Date;
            var today = DateTime.Today;

            if (dob > today)
            {
                return false;
            }

            return dob >= new DateTime(1900, 1, 1);
        }

        /// <summary>
        /// Validate Citizen ID Number: Must be exactly 12 digits
        /// </summary>
        /// <param name="citizenIdNumber"></param>
        /// <returns></returns>
        public bool IsValidCitizenIdNumber(string citizenIdNumber)
        {
            if (string.IsNullOrWhiteSpace(citizenIdNumber))
            {
                return false;
            }

            return Regex.IsMatch(citizenIdNumber, @"^\d{12}$");
        }
    }
}
