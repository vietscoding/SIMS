using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SIMS.Models;
using System.Reflection.Metadata.Ecma335;

// Lớp này chứa chìa khóa truy cập đến database và chứa các thao CRUD cơ bản lên các đối tượng trong SIMS (student, academic program,...)

namespace SIMS.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }



        public List<AcademicProgram> GetAllAcademicPrograms()
        {
            var academicProgram = new List<AcademicProgram>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Program;";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        academicProgram.Add(new AcademicProgram
                        {
                            AcademicProgramId = Convert.ToInt32(reader["program_id"]),
                            AcademicProgramName = reader["program_name"].ToString(),
                            MajorId = Convert.ToInt32(reader["major_id"]),
                            FacultyId = Convert.ToInt32(reader["faculty_id"]),
                            Language = reader["program_language"].ToString(),
                            Description = reader["program_description"].ToString(),
                            NumberOfSemester = Convert.ToInt32(reader["number_of_semester"]),
                            TotalOfRequiredCredits = Convert.ToInt32(reader["total_of_required_credits"]),
                            ObligatedCredits = Convert.ToInt32(reader["obligated_credits"]),
                            CreatedAt = reader["created_at"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["created_at"],
                            UpdatedAt = reader["updated_at"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["updated_at"],
                            IsDeleted = Convert.ToBoolean(reader["is_deleted"]),

                        });
                    }
                }
                return academicProgram;
            }
        }

        public List<Person> GetAllPeople()
        {
            var people = new List<Person>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Person;";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        people.Add(new Person
                        {
                            PersonId = reader.GetInt32(0).ToString(),
                            CitizenIdNumber = reader.GetString(1),
                            FullName = reader.GetString(4),
                            Gender = reader.GetBoolean(5),
                            DateOfBirth = reader.GetDateTime(6),
                            Email = reader.GetString(7),
                            PhoneNumber = reader.GetString(8),
                            Address = reader.GetString(9),
                            Nationality = reader.GetString(10),
                            Created = reader.GetDateTime(11),
                            Updated = reader.GetDateTime(12)

                        });
                    }
                }
            }

            return people;
        }

        public Student GetStudentById(int studentId)
        {
            Student student = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                                SELECT 
                                    p.person_id,
                                    p.citizen_id_number,
                                    p.full_name,
                                    p.gender,
                                    p.date_of_birth,
                                    p.email,
                                    p.phone_number,
                                    p.address,
                                    p.nationality,
                                    s.student_id,
                                    s.student_code,
                                    s.program_id,
                                    s.enrollment_date,
                                    s.current_semester,
                                    s.student_status_id,
                                    s.gpa,
                                    s.credits_earned,
                                    s.expected_graduate_date,
                                    s.phone_number_of_relatives
                                FROM Person p
                                JOIN Student s ON p.person_id = s.person_id
                                WHERE s.student_id = @StudentId;
                               ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            student = new Student
                            {
                                PersonId = reader["person_id"].ToString(),
                                CitizenIdNumber = reader["citizen_id_number"].ToString(),
                                FullName = reader["full_name"].ToString(),
                                Gender = reader["gender"] == DBNull.Value ? null : (bool?)reader.GetBoolean(reader.GetOrdinal("gender")),
                                DateOfBirth = reader["date_of_birth"] == DBNull.Value ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                                Email = reader["email"].ToString(),
                                PhoneNumber = reader["phone_number"].ToString(),
                                Address = reader["address"].ToString(),
                                Nationality = reader["nationality"].ToString(),
                                StudentId = Convert.ToInt32(reader["student_id"]),
                                StudentCode = reader["student_code"] == DBNull.Value ? "unknown" : reader["student_code"].ToString(),
                                ProgramId = Convert.ToInt32(reader["program_id"]),
                                EnrollmentDate = reader["enrollment_date"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["enrollment_date"],
                                CurrentSemester = reader["current_semester"] == DBNull.Value ? (byte)0 : (byte)reader["current_semester"],
                                StudentStatusId = reader["student_status_id"] == DBNull.Value ? 0 : (int)reader["student_status_id"],
                                GPA = reader["gpa"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["gpa"]),
                                CreditsEarned = reader["credits_earned"] == DBNull.Value ? (short)0 : (short)reader["credits_earned"],
                                ExpectedGraduateDate = reader["expected_graduate_date"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["expected_graduate_date"],
                                PhoneNumberOfRelatives = reader["phone_number_of_relatives"].ToString()
                            };
                        }
                    }
                }
            }        

            return student;

        }

        public List<Student> GetAllStudents()
        {
            var students = new List<Student>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                                SELECT 
                                    p.person_id,
                                    p.citizen_id_number,
                                    p.full_name,
                                    p.gender,
                                    p.date_of_birth,
                                    p.email,
                                    p.phone_number,
                                    p.address,
                                    p.nationality,
                                    s.student_id,
                                    s.student_code,
                                    s.program_id,
                                    s.enrollment_date,
                                    s.current_semester,
                                    s.student_status_id,
                                    s.gpa,
                                    s.credits_earned,
                                    s.expected_graduate_date,
                                    s.phone_number_of_relatives
                                FROM Person p
                                JOIN Student s ON p.person_id = s.person_id;
                                ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            PersonId = reader.GetInt32(0).ToString(),
                            CitizenIdNumber = reader.GetString(1),
                            FullName = reader.GetString(2),
                            Gender = reader.GetBoolean(3),
                            DateOfBirth = reader.GetDateTime(4),
                            Email = reader.GetString(5),
                            PhoneNumber = reader.GetString(6),
                            Address = reader.GetString(7),
                            Nationality = reader.GetString(8),
                            StudentId = reader.GetInt32(9),
                            //StudentCode = reader.GetString(10),
                            StudentCode = "unknown",
                            ProgramId = reader.GetInt32(11),
                            EnrollmentDate = reader.GetDateTime(12),
                            CurrentSemester = reader.GetByte(13),
                            StudentStatusId = reader.GetInt32(14),
                            GPA = reader.GetDecimal(15),
                            CreditsEarned = reader.GetInt16(16),
                            ExpectedGraduateDate = reader.GetDateTime(17),
                            PhoneNumberOfRelatives = reader.GetString(18),
                        });
                    }
                }
            }

            return students;

        }

        public bool UpdateStudent(Student student)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
            UPDATE Person
            SET phone_number = @PhoneNumber,
                address = @Address,
                updated = GETDATE()
            WHERE person_id = @PersonId;
        ";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", student.Address ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PersonId", student.PersonId);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                return rows > 0;
            }
        }


    }
}
