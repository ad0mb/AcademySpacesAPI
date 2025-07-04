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

    public async Task<List<StudentEntry>> GetStudentsAsync(int schoolId)
    {
        try
        {
            var students = new List<StudentEntry>();

            var dbStudents = await (from s in _context.Students
                join sp in _context.StudentParents on s.StudentId equals sp.StudentId
                where s.SchoolId == schoolId
                select new
                {
                    Student = s,
                    ParentId = sp.ParentId
                }).ToListAsync();

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
                        RoleId = student.Student.RoleId,
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

            return students;
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue retrieving students from the database", ex);
        }
    }
}