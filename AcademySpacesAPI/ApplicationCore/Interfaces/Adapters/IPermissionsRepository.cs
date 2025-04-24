using AcademySpacesAPI.ApplicationCore.DomainEntities;
using AcademySpacesAPI.Entities;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;

public interface IPermissionsRepository
{
    Task CreateRolePermissionAsync(CreateRolePermissionEntry rolePermissions);
}