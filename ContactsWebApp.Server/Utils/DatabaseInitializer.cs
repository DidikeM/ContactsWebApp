using Microsoft.Data.SqlClient;

namespace ContactsWebApp.Server.Utils
{
    public class DatabaseInitializer(IConfiguration configuration)
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        private readonly string _databaseName = configuration["DatabaseName"]!;

        public async Task InitializeDatabaseAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            if (!await DatabaseExistsAsync(connection, _databaseName))
            {
                await CreateDatabaseAsync(connection, _databaseName);
            }

            connection.ChangeDatabase(_databaseName);

            if (!await TablesExistsAsync(connection))
            {
                await CreateTablesAsync(connection);
            }
        }

        private static async Task<bool> DatabaseExistsAsync(SqlConnection connection, string databaseName)
        {
            var commandText = $"SELECT database_id FROM sys.databases WHERE Name = '{databaseName}'";
            using var command = new SqlCommand(commandText, connection);
            var result = await command.ExecuteScalarAsync();
            return result != null;
        }

        private static async Task CreateDatabaseAsync(SqlConnection connection, string databaseName)
        {
            var commandText = $"CREATE DATABASE {databaseName}";
            using var command = new SqlCommand(commandText, connection);
            await command.ExecuteNonQueryAsync();
        }

        private static async Task<bool> TablesExistsAsync(SqlConnection connection)
        {
            var commandText = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME IN ('Users', 'Contacts')";
            using var command = new SqlCommand(commandText, connection);
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result) == 2;
        }

        private static async Task CreateTablesAsync(SqlConnection connection)
        {
            var usersTableCommandText = @"
                CREATE TABLE Users (
                    Id INT PRIMARY KEY IDENTITY,
                    Name NVARCHAR(250) NOT NULL,
                    Surname NVARCHAR(250) NOT NULL,
                    Email NVARCHAR(250) NOT NULL UNIQUE,
                    Password NVARCHAR(250) NOT NULL
                )";
            using (var command = new SqlCommand(usersTableCommandText, connection))
            {
                await command.ExecuteNonQueryAsync();
            }

            var contactsTableCommandText = @"
                CREATE TABLE Contacts (
                    Id INT PRIMARY KEY IDENTITY,
                    Name NVARCHAR(250) NOT NULL,
                    Surname NVARCHAR(250) NOT NULL,
                    PhoneNumber NVARCHAR(250) NOT NULL,
                    UserId INT NOT NULL,
                    CONSTRAINT FK_Contacts_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
                )";
            using (var command = new SqlCommand(contactsTableCommandText, connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

    }
}
