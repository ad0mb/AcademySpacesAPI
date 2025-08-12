using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Infrastructure.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class PeriodsRepository : IPeriodsRepository
{
    
private readonly MyDbContext _context;

    public PeriodsRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<(List<PeriodEntry> periodsList, int totalCount)> GetPeriodsBySchoolIdAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm, int facultyId, int courseId, TimeOnly? startTime, TimeOnly? endTime)
    {
        List<PeriodEntry> periodsList = new List<PeriodEntry>();

        var query = from c in _context.Courses
            where c.SchoolId == schoolId 
            from p in c.Periods
            where (facultyId <= 0 || p.Teacher.FacultyId == facultyId) //Faculty filter
            && (courseId <= 0 || p.CourseId == courseId) //Course filter
            && (startTime == null || p.StartTime >= startTime) //Start time filter
            && (endTime == null || p.EndTime <= endTime) //End time filter
            
            && (string.IsNullOrEmpty(searchTerm)
                || (
                    p.Teacher != null && (
                        (p.Teacher.FirstName != null && p.Teacher.FirstName.ToLower().Contains(searchTerm))
                        || (p.Teacher.MiddleName != null && p.Teacher.MiddleName.ToLower().Contains(searchTerm))
                        || (p.Teacher.LastName != null && p.Teacher.LastName.ToLower().Contains(searchTerm)
                        )
                    )
                )
                || (
                    p.Course != null && (
                        (p.Course.CourseName != null && p.Course.CourseName.ToLower().Contains(searchTerm))
                        || (p.Course.CourseCode != null && p.Course.CourseCode.ToLower().Contains(searchTerm))
                        || (p.Location != null && p.Location.ToLower().Contains(searchTerm))
                    )
                )
            )
            orderby p.PeriodId
            select p;
        
        var totalCount = await query.CountAsync();
            
        if (pageSize > 0 && pageNumber > 0)
        {
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
        
        var dbPeriods = await query
            .Distinct()
            .Include(p => p.Teacher)
            .Include(p => p.Course)
            .ToListAsync();

        foreach (var period in dbPeriods)
        {
            periodsList.Add(new PeriodEntry
            {
                PeriodId = period.PeriodId,
                Teacher = period.Teacher == null ? null : new FacultyEntry
                {
                    FacultyId = period.Teacher.FacultyId,
                    FirstName = period.Teacher.FirstName,
                    MiddleName = period.Teacher.MiddleName,
                    LastName = period.Teacher.LastName,
                    PhoneNumber = period.Teacher.PhoneNumber,
                    Email = period.Teacher.Email,
                },
                Course = new CourseEntry
                {
                    CourseId = period.Course.CourseId,
                    CourseName = period.Course.CourseName,
                    CourseCode = period.Course.CourseCode,
                    CourseDescription = period.Course.CourseDescription,
                },
                Location = period.Location,
                Capacity = period.Capacity,
                DayOfWeek = period.DayOfWeek,
                StartTime = period.StartTime,
                EndTime = period.EndTime,
                DateCreated = period.DateCreated,
                DateUpdated = period.DateModified,
            });
        }

        return (periodsList, totalCount);
    }

    public async Task CreatePeriodAsync(PeriodEntry request)
    {
        throw new NotImplementedException("CreatePeriodAsync method is not implemented yet.");
    }
}