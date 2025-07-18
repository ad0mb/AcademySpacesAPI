using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using Infrastructure.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class CourseRepository : ICourseRepository
{

    private readonly MyDbContext _context;  
    
    public CourseRepository(MyDbContext context)
    {
        
        _context = context;
        
    }

    public async Task<List<CourseEntry>> GetCoursesBySchoolIdAsync(int schoolId)
    {
        try
        {
            var courses = new List<CourseEntry>();

            var dbCourses = await (from c in _context.Courses
                where c.SchoolId == schoolId
                select c).ToListAsync();

            foreach (var course in dbCourses)
            {
                courses.Add(new CourseEntry
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    CourseCode = course.CourseCode,
                    CourseDescription = course.CourseDescription
                });
            }

            return courses;
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue retrieving courses from the database", ex);
        }
    }
}