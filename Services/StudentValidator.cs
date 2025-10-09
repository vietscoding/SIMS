using SIMS.Models;
using System.Text.RegularExpressions;

namespace SIMS.Services
{
    public class StudentValidator
    {
        public bool IsValid(Student student)
        {
            return IsValidEmail(student.Email)
                && IsValidPhone(student.PhoneNumber ?? "")
                && !string.IsNullOrWhiteSpace(student.FullName);
        }

        public bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^[0-9]{9,11}$");
        }
    }
}
