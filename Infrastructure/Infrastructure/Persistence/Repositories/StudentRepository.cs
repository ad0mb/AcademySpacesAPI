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
    public async Task<(List<StudentEntry> studentList, int totalCount )> GetStudentsAsync(int schoolId, int cycleId, int pageSize, int pageNumber, string? searchTerm, int yearLevelId, bool noClassroom = false)
    {
        try
        {
            var students = new List<StudentEntry>();

            //TODO: Implement search filtering
            //TODO: Check all conflict checkers and filters and make sure they are not filtering without considering duplicates related to cycleIds
            var query = from s in _context.Students
                where s.SchoolId == schoolId
                
                    && (yearLevelId <= 0 || s.YearLevel == yearLevelId)
                    && (!noClassroom || !s.ClassroomStudents.Any(cs => cs.Classroom.CycleId == cycleId))
                    
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

    //TODO: Implement duplicate checking for email and phone
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

    public async Task UpdateStudentAsync(StudentEntry student)
    {
        try
        {
            var existingStudent = await (from s in _context.Students
                where s.StudentId == student.StudentId && s.SchoolId == student.SchoolId
                select s).FirstOrDefaultAsync();

            if (existingStudent == null)
            {
                throw new NotFoundException("Student not found");
            }

            existingStudent.YearLevel = student.YearLevelId;
            existingStudent.FirstName = student.FirstName;
            existingStudent.MiddleName = student.MiddleName;
            existingStudent.LastName = student.LastName;
            existingStudent.PhoneNumber = student.Phone;
            existingStudent.Email = student.Email;
            
            var result = await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue updating student in the database", ex);
        }
    }
}