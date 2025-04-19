using System.Text.Json;
using AcademySpacesAPI.Models;
using MySqlConnector;

namespace AcademySpacesAPI.Data.Auth;

public class HandlerRepo
{

    private readonly string _connectionString;
    
    public HandlerRepo(IConfiguration configuration)
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
        
        try
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
            throw new Exception("Error retrieving user permissions from db: " + ex.Message);
        }
    }
    
    
}


