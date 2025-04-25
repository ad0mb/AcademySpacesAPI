using AcademySpacesAPI.ApplicationCore.DomainEntities;
using AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;
using AcademySpacesAPI.Exceptions;
using AcademySpacesAPI.Infrastructure.Persistence.Context;
using AcademySpacesAPI.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

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
        try
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
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue adding role permission to the database", ex);
        }
    }

    public async Task<List<RolePermissionEntry>?> GetUserPermissionsAsync(string identityId, string userType)
    {
        try
        {
            var permissions = new List<RolePermissionEntry>();
            var rolePermissionsResult = new List<RolePermission>();

            if (userType == "faculty")
            {
                rolePermissionsResult = await (from f in _context.Faculties
                    join fr in _context.FacultyRoles on f.FacultyId equals fr.FacultyId into facultyRoles
                    from fr in facultyRoles.DefaultIfEmpty()
                    join rp in _context.RolePermissions on fr.RoleId equals rp.RoleId into rolePermissions
                    from rp in rolePermissions.DefaultIfEmpty()
                    where f.IdentityId == identityId
                    select rp).ToListAsync();
            }

            foreach (var permission in rolePermissionsResult)
            {
                var rolePermissionEntry = new RolePermissionEntry
                {
                    Id = permission.Id,
                    RoleId = permission.RoleId,
                    PermissionName = permission.PermissionName,
                    Create = permission.Create,
                    Delete = permission.Delete,
                    Update = permission.Update
                };
                permissions.Add(rolePermissionEntry);
            }

            return permissions;
        } 
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue retrieving role permissions from the database", ex);
        }
    }
}