using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models
{
    [Table("Student")]
    public class Student
    {
        private string _studentCode = string.Empty;

        [Key]
        [Column("student_id")]
        public int StudentId { get; set; }

        [Column("person_id")]
        public int PersonId { get; set; }

        [Column("student_code")]
        public string StudentCode
        {
            get => _studentCode;
            set => _studentCode = value ?? string.Empty;
        }

        [Column("program_id")]
        public int ProgramId { get; set; }

        [Column("enrollment_date")]
        public DateTime EnrollmentDate { get; set; } // ngày nhập học

        [Column("current_semester")]
        public byte CurrentSemester { get; set; } // học kỳ hiện tại

        [Column("student_status_id")]
        public int StudentStatusId { get; set; } // trạng thái sinh viên

        [Column("gpa")]
        public decimal GPA { get; set; } // điểm trung bình tích lũy

        [Column("credits_earned")]
        public short CreditsEarned { get; set; } // số tín chỉ đã tích lũy

        [Column("expected_graduate_date")]
        public DateTime ExpectedGraduateDate { get; set; } // ngày dự kiến tốt nghiệp

        [Column("phone_number_of_relatives")]
        public string PhoneNumberOfRelatives { get; set; } = string.Empty; // số đt người thân

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [NotMapped]
        public string StudentClassificationByYear { get; set; } = string.Empty;

        [ForeignKey(nameof(PersonId))]
        public virtual Person Person { get; set; } = null!;

        [NotMapped]
        public string CitizenIdNumber
        {
            get => Person?.CitizenIdNumber ?? string.Empty;
            set => EnsurePerson().CitizenIdNumber = value;
        }

        [NotMapped]
        public string FullName
        {
            get => Person?.FullName ?? string.Empty;
            set => EnsurePerson().FullName = value;
        }

        [NotMapped]
        public bool? Gender
        {
            get => Person?.Gender;
            set => EnsurePerson().Gender = value;
        }

        [NotMapped]
        public DateTime? DateOfBirth
        {
            get => Person?.DateOfBirth;
            set => EnsurePerson().DateOfBirth = value;
        }

        [NotMapped]
        public string Email
        {
            get => Person?.Email ?? string.Empty;
            set => EnsurePerson().Email = value;
        }

        [NotMapped]
        public string PhoneNumber
        {
            get => Person?.PhoneNumber ?? string.Empty;
            set => EnsurePerson().PhoneNumber = value;
        }

        [NotMapped]
        public string Address
        {
            get => Person?.Address ?? string.Empty;
            set => EnsurePerson().Address = value;
        }

        [NotMapped]
        public string Nationality
        {
            get => Person?.Nationality ?? string.Empty;
            set => EnsurePerson().Nationality = value;
        }

        public Student()
        {
            Person = new Person();
        }

        private Person EnsurePerson()
        {
            if (Person == null)
            {
                Person = new Person();
            }

            return Person;
        }

        [ForeignKey(nameof(ProgramId))]
        public virtual AcademicProgram Program { get; set; } = null!;

    }
}
