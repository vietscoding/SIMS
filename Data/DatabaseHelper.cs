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
                string query = "SELECT person_id, full_name FROM Person";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        people.Add(new Person
                        {
                            PersonId = reader.GetString(1),
                            FullName = reader.GetString(1),
                        });
                    }
                }
            }

            return people;
        }

    }
}
