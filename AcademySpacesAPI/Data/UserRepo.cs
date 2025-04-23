using AcademySpacesAPI.Exceptions;
using AcademySpacesAPI.Infrastructure.Persistence.Models;
using MySqlConnector;

namespace AcademySpacesAPI.Data;

public class UserRepo
{
    
    private readonly string _connectionString;
    private readonly RoleRepo _roleRepo;
    
    public UserRepo(IConfiguration configuration, RoleRepo roleRepo)
    {
        _connectionString = configuration.GetConnectionString("StagingConnection") ??
                            throw new InvalidOperationException("Connection string 'StagingConnection' not found.");
        _roleRepo = roleRepo;
    }

    public async Task<int> CreateFacultyAsync(Faculty data, int[] roleIds)
    {
        var facultyId = -1;
        
        await using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        const string queryString = "INSERT INTO faculty (school_id, identity_id, first_name, last_name, phone_number, email) VALUES (@schoolId, @identityId, @firstName, @lastName, @phoneNumber, @email)";
        await using var command = new MySqlCommand(queryString, connection);
        command.Parameters.AddWithValue("@schoolId", data.SchoolId);
        command.Parameters.AddWithValue("@identityId", data.IdentityId);
        command.Parameters.AddWithValue("@firstName", data.FirstName);
        command.Parameters.AddWithValue("@lastName", data.LastName);
        command.Parameters.AddWithValue("@phoneNumber", data.PhoneNumber);
        command.Parameters.AddWithValue("@email", data.Email);
        
        await command.ExecuteNonQueryAsync();
        
        const string queryString2 = "SELECT LAST_INSERT_ID() as faculty_id";
        await using var command2 = new MySqlCommand(queryString2, connection);

        await using var reader = await command2.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            facultyId = reader.GetInt32("faculty_id");
        }
        if (facultyId == 0)
        {
            throw new NoRowsAffectedException("No rows affected by faculty creation query.");
        }
        
        connection.Close(); //close connection to prevent memory leak (only close connections when calling more data access layers below it)

        if (roleIds.Length > 0)
        {
            foreach (var roleId in roleIds)
            {
                await AddRoleToFacultyAsync(facultyId, roleId);
            }
        }

        return facultyId;
    }

    public async Task<Faculty?> GetFacultyAsync(string identityId)
    {
        await using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        const string queryString = "SELECT * FROM faculty WHERE identity_id = @identityId";
        await using var command = new MySqlCommand(queryString, connection);
        command.Parameters.AddWithValue("@identityId", identityId);

        await using var reader = await command.ExecuteReaderAsync();

        while(await reader.ReadAsync())
        {
            return new Faculty
            {
                FacultyId = reader.GetInt32("faculty_id"),
                SchoolId = reader.GetInt32("school_id"),
                IdentityId = reader.GetString("identity_id"),
                FirstName = reader.GetString("first_name"),
                LastName = reader.GetString("last_name"),
                PhoneNumber = reader.IsDBNull(reader.GetOrdinal("phone_number")) ? null : reader.GetString("phone_number"),
                Email = reader.GetString("email"),
                CreatedAt = reader.GetDateTime("date_created"),
                UpdatedAt = reader.GetDateTime("date_modified")
            };
        }

        return null;
    }

    public async Task AddRoleToFacultyAsync(int facultyId, int roleId)
    {
        await using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string queryString = "INSERT INTO faculty_roles (faculty_id, role_id) VALUES (@facultyId, @roleId)";
        await using var command = new MySqlCommand(queryString, connection);
        command.Parameters.AddWithValue("@facultyId", facultyId);
        command.Parameters.AddWithValue("@roleId", roleId);
        
        await command.ExecuteNonQueryAsync();
    }
    
    
    
}