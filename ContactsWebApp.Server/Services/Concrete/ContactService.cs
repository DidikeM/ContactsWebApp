using ContactsWebApp.Server.Models;
using ContactsWebApp.Server.Services.Abstract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ContactsWebApp.Server.Services.Concrete
{
    public class ContactService(IConfiguration configuration) : IContactService
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        private readonly string _databaseName = configuration["DatabaseName"]!;

        public void AddContact(Contact contact)
        {
            using var connection = new SqlConnection(_connectionString);

            connection.Open();
            connection.ChangeDatabase(_databaseName);

            string query = "INSERT INTO Contacts (Name, Surname, PhoneNumber, UserId) VALUES (@Name, @Surname, @PhoneNumber, @UserId)";

            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Name", contact.Name);
            command.Parameters.AddWithValue("@Surname", contact.Surname);
            command.Parameters.AddWithValue("@PhoneNumber", contact.PhoneNumber);
            command.Parameters.AddWithValue("@UserId", contact.UserId);

            command.ExecuteNonQuery();
        }

        public void DeleteContact(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            connection.Open();
            connection.ChangeDatabase(_databaseName);

            string query = "DELETE FROM Contacts WHERE Id = @Id";

            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }

        public Contact GetContactById(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            connection.Open();
            connection.ChangeDatabase(_databaseName);

            string query = "SELECT * FROM Contacts WHERE Id = @Id";

            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();

            reader.Read();

            return new Contact
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Surname = reader.GetString(2),
                PhoneNumber = reader.GetString(3),
                UserId = reader.GetInt32(4)
            };
        }

        public List<Contact> GetContacts(int userId)
        {
            using var connection = new SqlConnection(_connectionString);

            connection.Open();
            connection.ChangeDatabase(_databaseName);

            string query = "SELECT * FROM Contacts WHERE UserId = @UserId";

            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserId", userId);


            using var reader = command.ExecuteReader();

            var contacts = new List<Contact>();

            while (reader.Read())
            {
                contacts.Add(new Contact
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Surname = reader.GetString(2),
                    PhoneNumber = reader.GetString(3),
                    UserId = reader.GetInt32(4)
                });
            }

            return contacts;
        }

        public void UpdateContact(Contact contact)
        {
            using var connection = new SqlConnection(_connectionString);

            connection.Open();
            connection.ChangeDatabase(_databaseName);

            string query = "UPDATE Contacts SET Name = @Name, Surname = @Surname, PhoneNumber = @PhoneNumber WHERE Id = @Id";

            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Name", contact.Name);
            command.Parameters.AddWithValue("@Surname", contact.Surname);
            command.Parameters.AddWithValue("@PhoneNumber", contact.PhoneNumber);
            command.Parameters.AddWithValue("@Id", contact.Id);

            command.ExecuteNonQuery();
        }
    }
}
