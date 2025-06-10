using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using EFCore.BulkExtensions;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class PermissionsRepository : IPermissionsRepository
{
    
    private readonly MyDbContext _context;
    
    public PermissionsRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task CreateRolePermissionAsync(RolePermissionEntry request)
    {
        try
        {
            var rolePermissions = new RolePermission
            {
                RoleId = request.RoleId,
                PermissionName = request.PermissionName,
                CanCreate = request.Create,
                CanDelete = request.Delete,
                CanUpdate = request.Update,
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

    public async Task<List<RolePermissionEntry>?> GetUserPermissionsByIdentityIdAsync(string identityId, string userType)
    {
        try
        {
            var permissions = new List<RolePermissionEntry>();
            var rolePermissionsResult = new List<RolePermission>();

            if (userType == "faculty")
            {
                rolePermissionsResult = await (from f in _context.Faculties
                    join fr in _context.FacultyRoles on f.FacultyId equals fr.FacultyId into facultyRoles
                    from fr in Enumerable.DefaultIfEmpty<FacultyRole>(facultyRoles)
                    join rp in _context.RolePermissions on fr.RoleId equals rp.RoleId into rolePermissions
                    from rp in Enumerable.DefaultIfEmpty<RolePermission>(rolePermissions)
                    where f.IdentityId == identityId
                    select rp).ToListAsync();
            }
            
            if (rolePermissionsResult == null || rolePermissionsResult.Count < 1)
            {
                return null;
            }

            foreach (var permission in rolePermissionsResult)
            {
                var rolePermissionEntry = new RolePermissionEntry
                {
                    Id = permission.Id,
                    RoleId = permission.RoleId,
                    PermissionName = permission.PermissionName,
                    Create = permission.CanCreate,
                    Delete = permission.CanDelete,
                    Update = permission.CanUpdate
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
    
    public async Task<List<RolePermissionEntry>?> GetUserPermissionsByRoleIdAsync(int RoleId)
    {
        try
        {
            var permissions = new List<RolePermissionEntry>();
            var rolePermissionsResult = await (from rp in _context.RolePermissions
                where rp.RoleId == RoleId
                select rp).ToListAsync();
            
            if (rolePermissionsResult == null || rolePermissionsResult.Count < 1)
            {
                return null;
            }

            foreach (var permission in rolePermissionsResult)
            {
                var rolePermissionEntry = new RolePermissionEntry
                {
                    Id = permission.Id,
                    RoleId = permission.RoleId,
                    PermissionName = permission.PermissionName,
                    Create = permission.CanCreate,
                    Delete = permission.CanDelete,
                    Update = permission.CanUpdate
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
    
    public async Task BulkUpdateOrInsertRolePermissionAsync(List<RolePermissionEntry> request)
    {
        try
        {
            var permissions = new List<RolePermission>();
            
            foreach (var permission in request)
            {
                permissions.Add(new RolePermission
                {
                    Id = permission.Id,
                    RoleId = permission.RoleId,
                    PermissionName = permission.PermissionName,
                    CanCreate = permission.Create,
                    CanDelete = permission.Delete,
                    CanUpdate = permission.Update,
                });
            }

            await _context.BulkInsertOrUpdateAsync(permissions, new BulkConfig
            {
                PreserveInsertOrder = false,
                SetOutputIdentity = true
            });
            
        }
        
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue updating role permission in the database", ex);
        }
    }
    
    public async Task DeleteRolePermissionAsync(List<int> roleIds)
    {
        try
        {
            var permissions = new List<RolePermission>();
            
            foreach (var roleId in roleIds)
            {
                permissions.Add(new RolePermission
                {
                    Id = roleId,
                });
            }

            await _context.BulkDeleteAsync(permissions);
        } 
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue deleting role permission from the database", ex);
        }
    }
}