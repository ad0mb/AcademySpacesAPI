using AcademySpacesAPI.ApplicationCore.DomainEntities;
using AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;
using AcademySpacesAPI.Context;
using AcademySpacesAPI.Entities;
using AcademySpacesAPI.Exceptions;

namespace AcademySpacesAPI.Infrastructure.Persistence.Repositories;

public class PermissionsRepository : IPermissionsRepository
{
    
    private readonly MyDbContext _context;
    
    public PermissionsRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task CreateRolePermissionAsync(CreateRolePermissionEntry request)
    {
        var rolePermissions = new RolePermission
        {
            RoleId = request.RoleId,
            PermissionName = request.PermissionName,
            Create = request.Create,
            Delete = request.Delete,
            Update = request.Update,
        };
        
        await _context.RolePermissions.AddAsync(rolePermissions);
        var result = await _context.SaveChangesAsync();
        if (result == 0)
        {
            throw new NoRowsAffectedException("Role permission not created");
        }
    }
}