using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using EFCore.BulkExtensions;
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
            var existingEmail = await (from p in _context.Parents
                where p.Email == parent.Email && p.SchoolId == parent.SchoolId
                select p.Email).FirstOrDefaultAsync();
            
            if (existingEmail == parent.Email && parent.Email != null && parent.Email.Length > 0)
            {
                throw new DuplicateEmailException("Email already exists");
            }
            
            var roleId = await (from r in _context.Roles
                where r.RoleName == "Parent" && r.SchoolId == parent.SchoolId
                select r.RoleId).FirstOrDefaultAsync();

            var newParent = new Parent
            {
                SchoolId = parent.SchoolId,
                FirstName = parent.FirstName,
                MiddleName = parent.MiddleName,
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
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue adding parent to the database", ex);
        }
    }

    public async Task<ParentEntry> GetParentByParentIdAsync(int parentId)
    {
        try
        {
            //TODO: Make it search by schoolId too
            var parent = await (from p in _context.Parents
                where p.ParentId == parentId
                select p).FirstOrDefaultAsync();

            if (parent == null)
            {
                throw new NotFoundException("Parent not found");
            }

            var parentEntry = new ParentEntry
            {
                ParentId = parent.ParentId,
                SchoolId = parent.SchoolId,
                FirstName = parent.FirstName,
                MiddleName = parent.MiddleName,
                LastName = parent.LastName,
                Phone = parent.PhoneNumber,
                Email = parent.Email
            };

            return parentEntry;
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue retrieving parent from the database", ex);
        }
    }

    public async Task<(List<ParentEntry> parentList, int totalCount)> GetParentsBySchoolIdAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm)
    {
        try
        {
            List<ParentEntry> parents = new List<ParentEntry>();

            IQueryable<Parent> query = from p in _context.Parents
                where p.SchoolId == schoolId
                
                && (
                    string.IsNullOrEmpty(searchTerm) 
                    || (
                        p != null && 
                        (
                            (p.FirstName != null && p.FirstName.ToLower().Contains(searchTerm)) ||
                            (p.MiddleName != null && p.MiddleName.ToLower().Contains(searchTerm)) || 
                            (p.LastName != null && p.LastName.ToLower().Contains(searchTerm))
                        )
                    )
                )
                
                orderby p.ParentId
                select p;

            var totalCount = await query.CountAsync();
            
            if (pageSize > 0 && pageNumber > 0)
            {
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }

            var dbParents = await query.ToListAsync();

            foreach (var parent in dbParents)
            {
                parents.Add(new ParentEntry
                {
                    ParentId = parent.ParentId,
                    FirstName = parent.FirstName,
                    MiddleName = parent.MiddleName,
                    LastName = parent.LastName,
                    Phone = parent.PhoneNumber,
                    Email = parent.Email
                });
            }

            return (parents, totalCount);
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue retrieving parents from the database", ex);
        }
    }

    //TODO: Add schoolId to existingParent query to ensure parent belongs to the school
    public async Task UpdateParentAsync(ParentEntry parent)
    {
        try
        {
            var existingParent = await (from p in _context.Parents
                where p.ParentId == parent.ParentId && p.SchoolId == parent.SchoolId
                select p).FirstOrDefaultAsync();

            if (existingParent == null)
            {
                throw new NotFoundException("Parent not found");
            }

            existingParent.FirstName = parent.FirstName;
            existingParent.MiddleName = parent.MiddleName;
            existingParent.LastName = parent.LastName;
            existingParent.PhoneNumber = parent.Phone;
            existingParent.Email = parent.Email;

            var result = await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Isseue updating parent in the database", ex);
        }
    }

    //TODO: Redo/verify all exceptions in all repos
    public async Task DeleteParentAsync(int schoolId, int parentId)
    {
        try
        {
            var parent = await (from p in _context.Parents
                where p.ParentId == parentId && p.SchoolId == schoolId
                select p).FirstOrDefaultAsync();

            if (parent == null)
            {
                throw new NotFoundException("Parent not found");
            }
            
            _context.Parents.Remove(parent);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                throw new NoRowsAffectedException("Parent not deleted");
            }
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue deleting parent from the database", ex);
        }
    }

    public async Task BulkLinkStudentToParentAsync(HashSet< int> parentIds, int studentId)
    {
        try
        {
            var entryToDeleteList = new List<StudentParent>();

            var entriesToDelete = await (from sp in _context.StudentParents
                where sp.StudentId == studentId && !parentIds.Contains(sp.ParentId)
                select sp.ParentId).ToListAsync();

            foreach (var parentId in entriesToDelete)
            {
                entryToDeleteList.Add(new StudentParent
                {
                    ParentId = parentId,
                    StudentId = studentId
                });
            }

            await _context.BulkDeleteAsync(entryToDeleteList);
            
            if (parentIds.Count > 0)
            {
                var entryList = new List<StudentParent>();
        
        
                foreach (var parentId in parentIds)
                {
                    if (parentId > 0)
                    {
                        entryList.Add(new StudentParent
                        {
                            ParentId = parentId,
                            StudentId = studentId
                        });
                    }
                }

                await _context.BulkInsertOrUpdateAsync(entryList, new BulkConfig
                {
                    PreserveInsertOrder = false,
                    SetOutputIdentity = true
                });
            }
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue linking student to parents in the database", ex);
        }
    }
}