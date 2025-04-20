using AcademySpacesAPI.Exceptions;
using AcademySpacesAPI.Models;
using AcademySpacesAPI.Models.DatabaseModels;
using MySqlConnector;

namespace AcademySpacesAPI.Data.Auth;

public class SchoolRepo
{
    private readonly string _connectionString;
    private readonly RoleRepo _roleRepo;
    
    public SchoolRepo(IConfiguration configuration, RoleRepo roleRepo)
    {
        _connectionString = configuration.GetConnectionString("StagingConnection") ??
                            throw new InvalidOperationException("Connection string 'StagingConnection' not found.");
        _roleRepo = roleRepo;
    }

    public async Task<int[]> CreateSchoolAsync(Schools data)
    {
        var schoolId = -1;

        await using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        const string queryString =
            "INSERT INTO schools (name, country_of_origin) VALUES (@schoolName, @countryOfOrigin)";
        await using var command = new MySqlCommand(queryString, connection);
        command.Parameters.AddWithValue("@schoolName", data.Name);
        command.Parameters.AddWithValue("@countryOfOrigin", data.CountryOfOrigin);
        
        await command.ExecuteNonQueryAsync();
        
        const string queryString2 = "SELECT LAST_INSERT_ID() as school_id"; //LAST_INSERT_ID() is session based so it will return the last inserted id for this open connection
        await using var command2 = new MySqlCommand(queryString2, connection);

        await using var reader = await command2.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            schoolId = reader.GetInt32("school_id");
        }

        if (schoolId == 0) //if 0 that means no rows were affected (get last id return 0 for no id added)
        {
            throw new NoRowsAffectedException("No rows affected by school registration query");
        }
        
        connection.Close(); //close connection to prevent memory leak (only close connections when calling more data access layers below it)

        var chiefAdminRoleId =
            await _roleRepo.CreateRoleAsync("ChiefAdministrator", schoolId, ["ChiefAdministrator:000"]);
        await _roleRepo.CreateRoleAsync("Administrator", schoolId, ["Administrator:000"]);
        await _roleRepo.CreateRoleAsync("Teacher", schoolId, []);
        await _roleRepo.CreateRoleAsync("Parent", schoolId, []);
        await _roleRepo.CreateRoleAsync("Student", schoolId, []);

        return [schoolId, chiefAdminRoleId];
    }
}