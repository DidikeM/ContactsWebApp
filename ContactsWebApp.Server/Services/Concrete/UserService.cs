using ContactsWebApp.Server.Services.Abstract;
using ContactsWebApp.Server.Models;
using Microsoft.Data.SqlClient;

namespace ContactsWebApp.Server.Services.Concrete
{
    public class UserService(IConfiguration configuration) : IUserService
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        private readonly string _databaseName = configuration["DatabaseName"]!;

        public bool CheckPassword(User user, string password)
        {
            return user.Password == password;
            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    connection.Open();
            //    var command = new SqlCommand("SELECT COUNT(1) FROM Users WHERE Email = @Email AND Password = @Password", connection);
            //    command.Parameters.AddWithValue("@Email", user.Email);
            //    command.Parameters.AddWithValue("@Password", password);

            //    var result = (int)command.ExecuteScalar();
            //    return result > 0;
            //}
        }

        public bool CreateUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.ChangeDatabase(_databaseName);
                var command = new SqlCommand("INSERT INTO Users (Email, Password, Name, Surname) VALUES (@Email, @Password, @Name, @Surname)", connection);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Surname", user.Surname);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public User GetUserByEmail(string email)
        {
            User user = null!;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.ChangeDatabase(_databaseName);
                var command = new SqlCommand("SELECT Id, Email, Password, Name, Surname FROM Users WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", email);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    user = new User
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Password = reader.GetString(reader.GetOrdinal("Password")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Surname = reader.GetString(reader.GetOrdinal("Surname"))
                    };
                }
            }
            return user;
        }
    }
}
