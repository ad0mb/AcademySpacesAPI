using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using EntityFramework.Exceptions.Common;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class CourseRepository : ICourseRepository
{

    private readonly MyDbContext _context;  
    
    public CourseRepository(MyDbContext context)
    {
        
        _context = context;
        
    }

    public async Task<(List<CourseEntry> courseList, int totalCount)> GetCoursesBySchoolIdAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm)
    {
        try
        {
            var courses = new List<CourseEntry>();

            IQueryable<Course> query = from c in _context.Courses
                where c.SchoolId == schoolId
                
                && (
                    string.IsNullOrEmpty(searchTerm)
                    || (
                        c != null && (
                            (c.CourseName != null && c.CourseName.ToLower().Contains(searchTerm))
                            || (c.CourseCode != null && c.CourseCode.ToLower().Contains(searchTerm))
                            )
                        )
                    )
                
                orderby c.CourseId
                select c;
            
            var totalCount = await query.CountAsync();
            
            if (pageSize > 0 && pageNumber > 0)
            {
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }

            var dbCourses = await query
                .ToListAsync();
            
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

            return (courses, totalCount);
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue retrieving courses from the database", ex);
        }
    }

    //TODO: Decide between handling duplicate foreign key violations through try or catch or check before adding (referencing student and faculty repositories)
    public async Task CreateCourseAsync(CourseEntry request)
    {
        try
        {
            var existingContraints = await (from c in _context.Courses
                where (c.CourseName == request.CourseName || c.CourseCode == request.CourseCode) && c.SchoolId == request.SchoolId
                select c).FirstOrDefaultAsync();
            
            if (existingContraints?.CourseCode == request.CourseCode || existingContraints?.CourseName == request.CourseName)
            {
                throw new DuplicateNameException("Course with the same name or code already exists");
            }
            
            var course = new Course
            {
                SchoolId = request.SchoolId,
                CourseName = request.CourseName,
                CourseCode = request.CourseCode,
                CourseDescription = request.CourseDescription,
            };

            await _context.Courses.AddAsync(course);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                throw new NoRowsAffectedException("Course not created");
            }
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue creating course in the database", ex);
        }
    }

    //TODO: Double check as it is a expensive operation (maximum of 3 queries!!!)
    public async Task UpdateCourseAsync(CourseEntry courseEntry)
    {
        try
        {
            var course = await (from c in _context.Courses
                where c.CourseId == courseEntry.CourseId 
                      && c.SchoolId == courseEntry.SchoolId
                select c).FirstOrDefaultAsync();

            if (course == null)
            {
                throw new NotFoundException("Course not found");
            }
            
            
            var existingContraints = await (from c in _context.Courses
                where (c.CourseName == courseEntry.CourseName || c.CourseCode == courseEntry.CourseCode) 
                      && c.SchoolId == courseEntry.SchoolId
                      && c.CourseId != courseEntry.CourseId
                select c).FirstOrDefaultAsync();
            
            if (existingContraints?.CourseCode == courseEntry.CourseCode || existingContraints?.CourseName == courseEntry.CourseName)
            {
                throw new DuplicateNameException("Course with the same name or code already exists");
            }
            
            course.CourseName = courseEntry.CourseName;
            course.CourseCode = courseEntry.CourseCode;
            course.CourseDescription = courseEntry.CourseDescription;
            
            var result = await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) 
        {
            throw new DbException("Issue updating course in the database", ex);
        }
    }

    public async Task DeleteCourseAsync(int schoolId, int courseId)
    {
        try
        {
            var course = await (from c in _context.Courses
                where c.CourseId == courseId && c.SchoolId == schoolId
                select c).FirstOrDefaultAsync();

            if (course == null)
            {
                throw new NotFoundException("Course not found");
            }
            
            _context.Courses.Remove(course);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                throw new NoRowsAffectedException("Course not deleted");
            }
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue deleting course from the database", ex);
        }
    }
}