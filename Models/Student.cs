namespace SIMS.Models
{
    public class Student : Person
    {
        public string StudentId { get; set; } = string.Empty;
        //public string StudentCode { get; set; } = string.Empty;
        private string _studentCode;

        public string StudentCode
        {
            get { return _studentCode; }
            set
            {
                if (value != null)
                    _studentCode = value;
                else
                    _studentCode = string.Empty;
            }
        }

        public string ProgramId { get; set; } = string.Empty; // năm học
        public DateTime EnrollmentDate { get; set; } // ngày nhập học
        public int CurrentSemester { get; set; } // học kỳ hiện tại
        public int StudentStatusId { get; set; } // trạng thái sinh viên (đang học, nghỉ học, tốt nghiệp, v.v.)
        public decimal GPA { get; set; } // điểm trung bình tích lũy
        public int CreditsEarned { get; set; } // số tín chỉ đã tích lũy
        public DateTime ExpectedGraduateDate { get; set; } // ngày dự kiến tốt nghiệp
        public string PhoneNumberOfRelatives { get; set; } = string.Empty; // loại số đt người thân (cha, mẹ, v.v.)

        /// <summary>
        /// Phân loại sinh viên dựa trên năm nhập học
        /// Năm nhất: Fresher
        /// Năm 2: Sophomore
        /// Năm 3: Junior
        /// Năm 4: Senior
        /// </summary>
        public string StudentClassificationByYear { get; set; } = string.Empty;


        public Student() { }

        public IEnumerable<Student> Students { get; private set; }


    }
}
