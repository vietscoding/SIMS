using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SIMS.Models;

namespace SIMS.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
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
                            StudentId = reader.GetInt32(9).ToString(),
                            //StudentCode = reader.GetString(10),
                            StudentCode = "unknown",
                            ProgramId = reader.GetInt32(11).ToString(),
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


    }
}
