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

    public async Task<List<PeriodEntry>> GetPeriodsBySchoolIdAsync(int schoolId)
    {
        List<PeriodEntry> periods = new List<PeriodEntry>();

        var dbPeriods = await (from c in _context.Courses
            where c.SchoolId == schoolId
            from p in c.Periods
            select p)
            .Distinct()
            .Include(p => p.Teacher)
            .Include(p => p.Course)
            .ToListAsync();

        foreach (var period in dbPeriods)
        {
            periods.Add(new PeriodEntry
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

        return periods;
    }
}