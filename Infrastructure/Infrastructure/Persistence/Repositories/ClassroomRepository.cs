using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class ClassroomRepository : IClassroomRepository
{

    private readonly MyDbContext _context;

    public ClassroomRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task CreateClassroomAsync(ClassroomEntry classroomEntry)
    {
        try
        {
            var classroom = new Classroom
            {
                SchoolId = classroomEntry.SchoolId,
                ClassroomTeacherId = classroomEntry.ClassroomTeacherId,
                ClassroomName = classroomEntry.ClassroomName,
            };
            
            await _context.Classrooms.AddAsync(classroom);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                throw new NoRowsAffectedException("Classroom not created");
            }
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue adding classroom to the database", ex);
        }
    }
    
    public async Task<(List<ClassroomEntry> classroomsList, int totalCount)> GetClassroomsAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm)
    {
        try
        {
            List<ClassroomEntry> classrooms = new List<ClassroomEntry>();

            var query =  from c in _context.Classrooms
                from f in _context.Faculties.Where(f => f.FacultyId == c.ClassroomTeacherId).DefaultIfEmpty()
                join cs in _context.ClassroomStudents on c.ClassroomId equals cs.ClassroomId into studentGroup
                where c.SchoolId == schoolId

                      //Search filter
                      && (
                          string.IsNullOrEmpty(searchTerm) // If searchTerm is null or empty, return all classrooms
                          || c.ClassroomName.ToLower().Contains(searchTerm) // Search by classroom name
                          || (
                              f != null &&
                              ( // Search by teacher's name
                                  (f.FirstName != null && f.FirstName.ToLower().Contains(searchTerm)) ||
                                  (f.MiddleName != null && f.MiddleName.ToLower().Contains(searchTerm)) ||
                                  (f.LastName != null && f.LastName.ToLower().Contains(searchTerm))
                              )
                          )
                      )

                orderby c.ClassroomId
                select new
                {
                    Classroom = c,
                    NumberOfStudents = studentGroup.Count(),
                    ClassroomTeacherName =
                        (f != null ? f.FirstName : "") +
                        (f != null && !string.IsNullOrEmpty(f.MiddleName) ? " " + f.MiddleName : "") +
                        (f != null && !string.IsNullOrEmpty(f.LastName) ? " " + f.LastName : "")
                };
            
            var totalCount = await query.CountAsync();
            
            if (pageSize > 0 && pageNumber > 0)
            {
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }

            var dbClassrooms = await query.ToListAsync();
            
            foreach (var classroom in dbClassrooms)
            {
                classrooms.Add(new ClassroomEntry
                {
                    ClassroomId = classroom.Classroom.ClassroomId,
                    ClassroomTeacherId = classroom.Classroom.ClassroomTeacherId,
                    ClassroomName = classroom.Classroom.ClassroomName,
                    NumberOfStudents = classroom.NumberOfStudents,
                    ClassroomTeacherName = classroom.ClassroomTeacherName
                });
            }

            return (classrooms, totalCount);
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue retrieving classrooms from the database", ex);
        }
    }
}