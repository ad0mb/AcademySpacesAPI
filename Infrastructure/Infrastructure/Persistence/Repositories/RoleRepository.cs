using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    
    private readonly MyDbContext _context;
    private readonly IPermissionsRepository _permissionsRepository;
    
    public RoleRepository(MyDbContext context, IPermissionsRepository permissionsRepository)
    {
        _context = context;
        _permissionsRepository = permissionsRepository;
    }

    public async Task<int> CreateRoleAsync(RoleEntry request)
    {
        try
        {
            var roleName =
                await (from r in _context.Roles
                    where r.SchoolId == request.SchoolId && r.RoleName == request.RoleName
                    select r.RoleName).FirstOrDefaultAsync();

            if (roleName == request.RoleName)
            {
                throw new DuplicateNameException("A role with this name already exists.");
            }
            
            var role = new Role
            {
                SchoolId = request.SchoolId,
                RoleName = request.RoleName,
                RoleDescription = request.RoleDescription,
            };

            await _context.Roles.AddAsync(role);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                throw new NoRowsAffectedException("No rows were affected when creating the role.");
            }
            
            return role.RoleId;
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue adding role to the database", ex);
        }
    }

    public async Task<int> CreateRoleAsync(RoleEntry request, string[] permissions)
    {
        try
        {
            var roleName =
                await (from r in _context.Roles
                    where r.SchoolId == request.SchoolId && r.RoleName == request.RoleName
                    select r.RoleName).FirstOrDefaultAsync();

            if (roleName == request.RoleName)
            {
                throw new DuplicateNameException("A role with this name already exists.");
            }
            
            var role = new Role
            {
                SchoolId = request.SchoolId,
                RoleName = request.RoleName,
                RoleDescription = request.RoleDescription,
            };

            await _context.Roles.AddAsync(role);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                throw new NoRowsAffectedException("No rows were affected when creating the role.");
            }
            
            foreach (var permission in permissions)
            {
                await _permissionsRepository.CreateRolePermissionAsync(new RolePermissionEntry
                {
                    RoleId = role.RoleId,
                    PermissionName = permission.Split(":")[0],
                    Create = permission.Split(":")[1].ToCharArray()[0] == '1',
                    Delete = permission.Split(":")[1].ToCharArray()[1] == '1',
                    Update = permission.Split(":")[1].ToCharArray()[2] == '1',
                });
            }

            return role.RoleId;
        } 
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue adding role to the database", ex);
        }
    }
    
    public async Task<List<RoleEntry>> GetRolesBySchoolIdAsync(int schoolId)
    {
        
        List<RoleEntry> roles = new List<RoleEntry>();
        try
        {
            var dbRoles = await (from r in _context.Roles
                where r.SchoolId == schoolId
                select r).ToListAsync();

            foreach (var role in dbRoles)
            {
                roles.Add(new RoleEntry
                {
                    RoleId = role.RoleId,
                    SchoolId = role.SchoolId,
                    RoleName = role.RoleName,
                    RoleDescription = role.RoleDescription,
                    DateCreated = role.DateCreated,
                    DateUpdated = role.DateModified
                });
            }
            
            return roles;
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue retrieving roles from the database", ex);
        }
    }

    public async Task UpdateRoleAsync(RoleEntry request)
    {
        try
        {
            var role =
                await (from r in _context.Roles
                    where r.RoleId == request.RoleId
                    select r).FirstOrDefaultAsync();

            if (role == null)
            {
                throw new NotFoundException("Role to update not found.");
            }
            
            var roleName =
                await (from r in _context.Roles
                    where r.SchoolId == request.SchoolId && r.RoleName == request.RoleName
                    select r.RoleName).FirstOrDefaultAsync();
            
            if (request.RoleName != role.RoleName && roleName == request.RoleName)
            {
                throw new DuplicateNameException("A role with this name already exists.");
            }
            
            role.RoleName = request.RoleName;
            role.RoleDescription = request.RoleDescription;
            
            var result = await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue updating role in the database", ex);
        }
    }
}