using System.Text.Json;
using AcademySpacesAPI.Models;
using MySqlConnector;

namespace AcademySpacesAPI.Data.Auth;

public class PermissionsRepo
{

    private readonly string _connectionString;
    
    public PermissionsRepo(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("StagingConnection") ??
                            throw new InvalidOperationException("Connection string 'StagingConnection' not found.");
    }
    
    public async Task<List<RolePermissions>?> GetUserPermissionsAsync(string identityId, string userType)
    {
        var permissions = new List<RolePermissions>();
        string queryString;

        //TODO: Untested query string (the first one for students and parents not faculty one)
        if (userType == "faculty")
        {
            queryString = "" +
                          "SELECT rp.* " +
                          "FROM faculty f " +
                          "RIGHT JOIN faculty_roles fr ON f.faculty_id = fr.faculty_id " +
                          "RIGHT JOIN role_permissions rp ON fr.role_id = rp.role_id " +
                          "WHERE f.identity_id = @identityId";
        }
        else
        {
            queryString = "SELECT rp.* " +
                          $"FROM {userType} ut " +
                          "RIGHT JOIN role_permissions rp ON ut.role_id = rp.role_id " +
                          "WHERE ut.identity_id = @identityId ";
        }
        
        try //using try-catch becasue AuthenticationHandler cannot afford exception break (untested result if it does)
        {
            await using var
                connection =
                    new MySqlConnection(
                        _connectionString); //will automatically dispose of connection at the end of scope (method in this case) //The await statement is applied to the using (disposal flow)
            connection.Open();

            await using var command = new MySqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@identityId", identityId);
            await using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                var rolePermission = new RolePermissions
                {
                    Id = reader.GetInt32("id"),
                    RoleId = reader.GetInt32("role_id"),
                    PermissionName = reader.GetString("permission_name"),
                    Create = reader.GetBoolean("create"),
                    Update = reader.GetBoolean("update"),
                    Delete = reader.GetBoolean("delete")
                };
                permissions.Add(rolePermission);
            }

            if (permissions.Count > 0)
            {
                return permissions;
            }

            return null;
        } 
        catch (Exception ex)
        {
            //TODO: Recheck if you should even throw an exception here and instead handle it in the FirebaseAuthService 
            throw new Exception("Error retrieving user permissions from db: " + ex.Message);
        }
    }
    
    public async Task<List<RolePermissions>?> GetRolePermissionsAsync(int roleId)
    {
        var permissions = new List<RolePermissions>();

        await using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        string queryString = "SELECT * FROM role_permissions WHERE role_id = @roleId";

        await using var command = new MySqlCommand(queryString, connection);
        command.Parameters.AddWithValue("@roleId", roleId);
        await using var reader = await command.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            var rolePermission = new RolePermissions
            {
                Id = reader.GetInt32("id"),
                RoleId = reader.GetInt32("role_id"),
                PermissionName = reader.GetString("permission_name"),
                Create = reader.GetBoolean("create"),
                Update = reader.GetBoolean("update"),
                Delete = reader.GetBoolean("delete"),
                CreatedAt = reader.GetDateTime("date_created"),
                UpdatedAt = reader.GetDateTime("date_modified")
            };
            permissions.Add(rolePermission);
        }

        if (permissions.Count > 0)
        {
            return permissions;
        }

        return null;
    }

    public async Task CreateRolePermissionAsync(string permissionString, int roleId)
    {
        var permissionName = permissionString.Split(":")[0];
        var permissions = permissionString.Split(":")[1].ToCharArray();
        
        await using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        
        const string queryString =
            "INSERT INTO role_permissions (role_id, permission_name, `create`, `delete`, `update`) VALUES (@roleId, @permissionName, @create, @delete, @update)"; // create delete and update have ` on them because create delete and update are reserved keywords and ` is an escape character
        await using var command = new MySqlCommand(queryString, connection);
        command.Parameters.AddWithValue("@roleId", roleId);
        command.Parameters.AddWithValue("@permissionName", permissionName);
        command.Parameters.AddWithValue("@create", permissions[0] == '1');
        command.Parameters.AddWithValue("@delete", permissions[1] == '1');
        command.Parameters.AddWithValue("@update", permissions[2] == '1');
        
        await command.ExecuteNonQueryAsync();
    }
}


