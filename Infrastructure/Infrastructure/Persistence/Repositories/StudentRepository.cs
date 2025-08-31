using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class StudentRepository : IStudentRepository
{
    
    private readonly MyDbContext _context;

    public StudentRepository(MyDbContext context)
    {
        _context = context;
    }

    //TODO: Comeback and implement distinct or a better way to deal with duplicate entries due to parent Ids (inefficient looping perhaps)
    public async Task<(List<StudentEntry> studentList, int totalCount )> GetStudentsAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm, int yearLevelId)
    {
        try
        {
            var students = new List<StudentEntry>();

            //TODO: Implement search filtering
            var query = from s in _context.Students
                where s.SchoolId == schoolId
                
                    && (yearLevelId <= 0 || s.YearLevel == yearLevelId)
                
                
                select new
                {
                    Student = s,
                    ParentIds = new HashSet<int>(s.StudentParents.Select(sp => sp.ParentId))
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
                    ParentIds = student.ParentIds
                });
            }

            return (students, totalCount);
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue retrieving students from the database", ex);
        }
    }

    //TODO: Implement dupliate checking for email and phone
    public async Task<int> CreateStudentAsync(StudentEntry student)
    {
        try
        {
            var dbStudent = new Student
            {
                SchoolId = student.SchoolId,
                YearLevel = student.YearLevelId,
                FirstName = student.FirstName,
                MiddleName = student.MiddleName,
                LastName = student.LastName,
                PhoneNumber = student.Phone,
                Email = student.Email,
            };
            
            await _context.Students.AddAsync(dbStudent);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                throw new NoRowsAffectedException("Student not created");
            }

            return dbStudent.StudentId;
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue creating student in the database", ex);
        }
    }
}