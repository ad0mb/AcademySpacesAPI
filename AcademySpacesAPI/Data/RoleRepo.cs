using AcademySpacesAPI.Exceptions;
using AcademySpacesAPI.Models.DatabaseModels;
using MySqlConnector;

namespace AcademySpacesAPI.Data.Auth;

public class RoleRepo
{
    private readonly string _connectionString;
    private readonly PermissionsRepo _permissionsRepo;
    
    public RoleRepo(IConfiguration configuration, PermissionsRepo permissionsRepo)
    {
        _connectionString = configuration.GetConnectionString("StagingConnection") ??
                            throw new InvalidOperationException("Connection string 'StagingConnection' not found.");
        _permissionsRepo = permissionsRepo;
    }
    
    public async Task<int> CreateRoleAsync(string roleName, int schoolId, string[] permissions)
    {
        var roleId = -1;
        
        await using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string queryString =
            "INSERT INTO roles (school_id, role_name) VALUES (@schoolId, @roleName)";
        await using var command = new MySqlCommand(queryString, connection);
        command.Parameters.AddWithValue("@schoolId", schoolId);
        command.Parameters.AddWithValue("@roleName", roleName);
        
        await command.ExecuteNonQueryAsync();
        
        const string queryString2 = "SELECT LAST_INSERT_ID() as role_id";
        await using var command2 = new MySqlCommand(queryString2, connection);
        
        await using var reader = await command2.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            roleId = reader.GetInt32("role_id");
        }
        if (roleId == 0) 
        {
            throw new NoRowsAffectedException("No rows affected by role creation query.");
        }
        
        connection.Close(); //close connection to prevent memory leak (only close connections when calling more data access layers below it)
        
        if (permissions.Length > 0)
        {
            foreach (var permission in permissions)
            {
                await _permissionsRepo.CreateRolePermissionAsync(permission, roleId);
            }
        }

        return roleId;
    }

    public async Task<List<Roles>> GetRolesBySchoolIdAsync(int schoolId)
    {
        var roles = new List<Roles>();
        
        await using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string queryString = "SELECT * FROM roles WHERE school_id = @schoolId";
        await using var command = new MySqlCommand(queryString, connection);
        command.Parameters.AddWithValue("@schoolId", schoolId);
        
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var role = new Roles
            {
                RoleId = reader.GetInt32("role_id"),
                SchoolId = reader.GetInt32("school_id"),
                RoleName = reader.GetString("role_name"),
                CreatedAt = reader.GetDateTime("date_created"),
                UpdatedAt = reader.GetDateTime("date_modified")
            };
            roles.Add(role);
        }
        
        if (roles.Count > 0) 
        {
            return roles;
        }

        throw new Exception("No roles found for school (NOT SUPPOSED TO HAPPEN, SCHOOL HAS 5 DEFAULT ROLES)");
    }
    
    
}