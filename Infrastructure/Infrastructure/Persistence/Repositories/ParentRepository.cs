using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class ParentRepository : IParentRepository
{
    private readonly MyDbContext _context;
    
    public ParentRepository(MyDbContext context)
    {
        _context = context;
    }
    
    public async Task CreateParentAsync(ParentEntry parent)
    {
        try
        {
            var roleId = await (from r in _context.Roles
                where r.RoleName == "Parent" && r.SchoolId == parent.SchoolId
                select r.RoleId).FirstOrDefaultAsync();

            var newParent = new Parent
            {
                SchoolId = parent.SchoolId,
                RoleId = roleId,
                FirstName = parent.FirstName,
                LastName = parent.LastName,
                PhoneNumber = parent.Phone,
                Email = parent.Email,
            };

            await _context.Parents.AddAsync(newParent);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                throw new NoRowsAffectedException("Now rows were affected when creating the parent.");
            }
        } catch (DbUpdateException ex)
        {
            throw new DbException("Issue adding parent to the database", ex);
        }
    }
}