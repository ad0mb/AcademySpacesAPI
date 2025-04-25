using AcademySpacesAPI.ApplicationCore.DomainEntities;
using AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;
using AcademySpacesAPI.Exceptions;
using AcademySpacesAPI.Infrastructure.Persistence.Context;
using AcademySpacesAPI.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AcademySpacesAPI.Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    
    private readonly MyDbContext _context;
    private readonly IPermissionsRepository _permissionsRepository;
    
    public RoleRepository(MyDbContext context, IPermissionsRepository permissionsRepository)
    {
        _context = context;
        _permissionsRepository = permissionsRepository;
    }

    public async Task<int> CreateRoleAsync(CreateRoleEntry request)
    {
        try
        {
            var role = new Role
            {
                SchoolId = request.SchoolId,
                RoleName = request.RoleName,
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

    public async Task<int> CreateRoleAsync(CreateRoleEntry request, string[] permissions)
    {
        try
        {
            var role = new Role
            {
                SchoolId = request.SchoolId,
                RoleName = request.RoleName,
            };

            await _context.Roles.AddAsync(role);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                throw new NoRowsAffectedException("No rows were affected when creating the role.");
            }

            if (!permissions.IsNullOrEmpty())
            {
                foreach (var permission in permissions)
                {
                    await _permissionsRepository.CreateRolePermissionAsync(new CreateRolePermissionEntry
                    {
                        RoleId = role.RoleId,
                        PermissionName = permission.Split(":")[0],
                        Create = permission.Split(":")[1].ToCharArray()[0] == '1',
                        Delete = permission.Split(":")[1].ToCharArray()[1] == '1',
                        Update = permission.Split(":")[1].ToCharArray()[2] == '1',
                    });
                }
            }

            return role.RoleId;
        } 
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue adding role to the database", ex);
        }
    }
}