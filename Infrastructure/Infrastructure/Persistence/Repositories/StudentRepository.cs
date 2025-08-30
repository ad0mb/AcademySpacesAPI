using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using Infrastructure.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class StudentRepository : IStudentRepository
{
    
    private readonly MyDbContext _context;

    public StudentRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<(List<StudentEntry> studentList, int totalCount )> GetStudentsAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm, int yearLevelId)
    {
        try
        {
            var students = new List<StudentEntry>();

            //TODO: Implement search filtering
            var query = from s in _context.Students
                join sp in _context.StudentParents on s.StudentId equals sp.StudentId
                where s.SchoolId == schoolId
                
                    && (yearLevelId <= 0 || s.YearLevel == yearLevelId)
                
                
                select new
                {
                    Student = s,
                    ParentId = sp.ParentId
                };
            
            var totalCount = await query.CountAsync();

            if (pageSize > 0 && pageNumber > 0)
            {
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }

            var dbStudents = await query
                .ToListAsync();

            foreach (var student in dbStudents)
            {
                var added = false;
                
                foreach (var student2 in students)
                {
                    if (student2.StudentId == student.Student.StudentId)
                    {
                        student2.ParentIds.Add(student.ParentId);
                        added = true;
                    }
                }

                if (!added)
                {
                    students.Add(new StudentEntry
                    {
                        StudentId = student.Student.StudentId,
                        SchoolId = student.Student.SchoolId,
                        YearLevelId = student.Student.YearLevel,
                        IdentityId = student.Student.IdentityId,
                        FirstName = student.Student.FirstName,
                        MiddleName = student.Student.MiddleName,
                        LastName = student.Student.LastName,
                        Phone = student.Student.PhoneNumber,
                        Email = student.Student.Email,
                        DateCreated = student.Student.DateCreated,
                        DateUpdated = student.Student.DateModified,
                        ParentIds = new List<int>() { student.ParentId }
                    });
                }
            }

            return (students, totalCount);
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue retrieving students from the database", ex);
        }
    }
}