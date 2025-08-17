using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class PeriodsRepository : IPeriodsRepository
{
    
private readonly MyDbContext _context;

    public PeriodsRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<(List<PeriodEntry> periodsList, int totalCount)> GetPeriodsBySchoolIdAsync(int schoolId, int cycleId, int pageSize, int pageNumber, string? searchTerm, int facultyId, int courseId, TimeOnly? startTime, TimeOnly? endTime)
    {
        List<PeriodEntry> periodsList = new List<PeriodEntry>();

        try
        {
            var query = from c in _context.Courses
                where c.SchoolId == schoolId
                from p in c.Periods
                where p.CycleId == cycleId
                      && (facultyId <= 0 || p.Teacher.FacultyId == facultyId) //Faculty filter
                      && (courseId <= 0 || p.CourseId == courseId) //Course filter
                      && (startTime == null || p.StartTime >= startTime) //Start time filter
                      && (endTime == null || p.EndTime <= endTime) //End time filter

                      && (string.IsNullOrEmpty(searchTerm)
                          || (
                              p.Teacher != null && (
                                  (p.Teacher.FirstName != null && p.Teacher.FirstName.ToLower().Contains(searchTerm))
                                  || (p.Teacher.MiddleName != null &&
                                      p.Teacher.MiddleName.ToLower().Contains(searchTerm))
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
                    Teacher = period.Teacher == null
                        ? null
                        : new FacultyEntry
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
                    Name = period.Name,
                    Location = period.Location,
                    DayOfWeek = period.DayOfWeek,
                    StartTime = period.StartTime,
                    EndTime = period.EndTime,
                    DateCreated = period.DateCreated,
                    DateUpdated = period.DateModified,
                });
            }

            return (periodsList, totalCount);
        } 
        catch (DbUpdateException ex)
        {
            throw new DbException("An error occurred while retrieving periods.", ex);
        }
    }

    public async Task CreatePeriodAsync(PeriodEntry request)
    {
        try
        {
            var conflictingPeriods = await (from p in _context.Periods 
                where p.CycleId == request.CycleId
                    && (request.TeacherId <= 0 || p.TeacherId == request.TeacherId) 
                    && (
                        (request.StartTime == null || request.EndTime == null || request.DayOfWeek <= 0) 
                        || 
                        (p.StartTime <= request.EndTime && request.StartTime <= p.EndTime && p.DayOfWeek == request.DayOfWeek)
                        )
                    select p).ToListAsync();

            if (conflictingPeriods.Any())
            {
                throw new SchedulingConflictException("A period with the same teacher, day of week, and time already exists.");
            }
            
            var period = new Period
            {
                CycleId = request.CycleId,
                TeacherId = request.TeacherId,
                CourseId = request.CourseId,
                Name = request.Name,
                Location = request.Location,
                DayOfWeek = request.DayOfWeek,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
            };

            await _context.Periods.AddAsync(period);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                throw new NoRowsAffectedException("Period not created");
            }
            
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue adding period to the database", ex);
        }
    }
}