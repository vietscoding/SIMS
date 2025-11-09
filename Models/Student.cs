namespace SIMS.Models
{
    public class Student : Person
    {
        // get program

        // getProgramById()

        public int StudentId { get; set; } = 0;
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

        

        public int ProgramId { get; set; } = 0;
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

    }
}
