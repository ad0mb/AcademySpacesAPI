using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using EFCore.BulkExtensions;
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

    //TODO: Just come back and check this logic again especially conflict checking
    public async Task<(List<PeriodEntry> periodsList, int totalCount)> GetPeriodsBySchoolIdAsync(int schoolId,
        int cycleId, int pageSize, int pageNumber, string? searchTerm, int facultyId, int courseId, TimeOnly? startTime,
        TimeOnly? endTime, int[] dayOfWeek, bool excludeClassroomId = false, bool onlyScheduled = false)
    {
        List<PeriodEntry> periodsList = new List<PeriodEntry>();

        try
        {
            var query = from p in _context.Periods
                join ps in _context.PeriodSchedules on p.PeriodId equals ps.PeriodId into psGroup
                from ps in psGroup.DefaultIfEmpty()
                where p.CycleId == cycleId
                      && (facultyId <= 0 || p.Teacher.FacultyId == facultyId) //Faculty filter
                      && (courseId <= 0 || p.CourseId == courseId) //Course filter

                      // Time and day filtering on PeriodSchedule
                      && (startTime == null || ps.StartTime >= startTime)
                      && (endTime == null || ps.EndTime <= endTime)
                      && (dayOfWeek.Length <= 0 || dayOfWeek.Sum() <= 0 || dayOfWeek.Contains(ps.DayOfWeek))
                      
                      // Only scheduled periods: must have at least one PeriodSchedule
                      && (!onlyScheduled || _context.PeriodSchedules.Any(s => s.PeriodId == p.PeriodId))

                      && (!excludeClassroomId ||
                          !_context.ClassroomSchedules.Any(cs => cs.PeriodId == p.PeriodId))
                      
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
                .Include(p => p.PeriodSchedules)
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
                    PeriodSchedule = period.PeriodSchedules
                        .Select(ps => new PeriodScheduleEntry
                        {
                            Id = ps.Id,
                            DayOfWeek = ps.DayOfWeek,
                            StartTime = ps.StartTime,
                            EndTime = ps.EndTime,
                        }).ToList(),
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

    //TODO: Just come back and check this logic again especially conflict checking
    public async Task CreatePeriodAsync(PeriodEntry request)
    {
        try
        {
            
            if (request.PeriodSchedule != null && request.PeriodSchedule.Count > 0 && request.TeacherId > 0) //conflict checker
            {
                var scheduleTuples = request.PeriodSchedule
                    .Select(ps => new { ps.DayOfWeek, ps.StartTime, ps.EndTime, ps.Id })
                    .ToList();
                
                var possibleConflicts = await (from p in _context.Periods
                    join ps in _context.PeriodSchedules on p.PeriodId equals ps.PeriodId
                    where p.CycleId == request.CycleId
                          && p.TeacherId == request.TeacherId
                    select new { ps.Id, ps.DayOfWeek, ps.StartTime, ps.EndTime }).ToListAsync();

                var hasServerConflict = scheduleTuples.Any(tuples =>
                    possibleConflicts.Any(dbEntries =>
                        tuples.DayOfWeek == dbEntries.DayOfWeek
                        && dbEntries.Id != tuples.Id
                        && dbEntries.StartTime < tuples.EndTime
                        && tuples.StartTime < dbEntries.EndTime
                    ));

                var hasRequestConflict = scheduleTuples
                    .Select((outer, outerIndex) => new { outer, innerIndex = outerIndex })
                    .Any(x => scheduleTuples
                        .Select((inner, innerIndex) => new { inner, innerIndex })
                        .Any(y =>
                            x.innerIndex != y.innerIndex &&
                            x.outer.DayOfWeek == y.inner.DayOfWeek &&
                            x.outer.StartTime < y.inner.EndTime &&
                            y.inner.StartTime < x.outer.EndTime
                        )
                    );

                var hasConflict = hasServerConflict || hasRequestConflict;

                if (hasConflict)
                {
                    throw new SchedulingConflictException(
                        "A period with the same teacher, day of week, and overlapping time already exists.");
                }
            }

            var period = new Period
            {
                CycleId = request.CycleId,
                TeacherId = request.TeacherId,
                CourseId = request.CourseId,
                Name = request.Name,
                Location = request.Location,
                PeriodSchedules = request.PeriodSchedule
                    .Select(ps => new PeriodSchedule
                    {
                        DayOfWeek = ps.DayOfWeek,
                        StartTime = ps.StartTime,
                        EndTime = ps.EndTime,
                    }).ToList()
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

    //TODO: Just come back and check this logic again especially conflict checking
    public async Task UpdatePeriodAsync(PeriodEntry request)
    {
        try
        {
            var period = await (from p in _context.Periods
                where p.PeriodId == request.PeriodId && p.CycleId == request.CycleId
                select p).FirstOrDefaultAsync();

            if (period == null)
            {
                throw new NotFoundException("Period not found");
            }

            if (request.PeriodSchedule != null && request.PeriodSchedule.Count > 0 && request.TeacherId > 0) //conflict checker
            {
                var scheduleTuples = request.PeriodSchedule
                    .Select(ps => new { ps.DayOfWeek, ps.StartTime, ps.EndTime, ps.Id })
                    .ToList();
                
                var possibleConflicts = await (from p in _context.Periods
                    join ps in _context.PeriodSchedules on p.PeriodId equals ps.PeriodId
                    where p.CycleId == request.CycleId
                          && p.TeacherId == request.TeacherId
                    select new { ps.Id, ps.DayOfWeek, ps.StartTime, ps.EndTime }).ToListAsync();

                var hasServerConflict = scheduleTuples.Any(tuples =>
                    possibleConflicts.Any(dbEntries =>
                        tuples.DayOfWeek == dbEntries.DayOfWeek
                        && dbEntries.Id != tuples.Id
                        && dbEntries.StartTime < tuples.EndTime
                        && tuples.StartTime < dbEntries.EndTime
                    ));

                var hasRequestConflict = scheduleTuples
                    .Select((outer, outerIndex) => new { outer, innerIndex = outerIndex })
                    .Any(x => scheduleTuples
                        .Select((inner, innerIndex) => new { inner, innerIndex })
                        .Any(y =>
                            x.innerIndex != y.innerIndex &&
                            x.outer.DayOfWeek == y.inner.DayOfWeek &&
                            x.outer.StartTime < y.inner.EndTime &&
                            y.inner.StartTime < x.outer.EndTime
                        )
                    );

                var hasConflict = hasServerConflict || hasRequestConflict;

                if (hasConflict)
                {
                    throw new SchedulingConflictException(
                        "A period with the same teacher, day of week, and overlapping time already exists.");
                }
            }

            period.TeacherId = request.TeacherId;
            period.CourseId = request.CourseId;
            period.Name = request.Name;
            period.Location = request.Location;
            
            var result = await _context.SaveChangesAsync();
            
            var periodSchedule = new List<PeriodSchedule>();
            
            foreach (var ps in request.PeriodSchedule)
            {
                var periodScheduleEntry = new PeriodSchedule
                {
                    Id = ps.Id,
                    PeriodId = ps.PeriodId,
                    DayOfWeek = ps.DayOfWeek,
                    StartTime = ps.StartTime,
                    EndTime = ps.EndTime,
                    DateUpdated = DateTime.Now
                };

                if (ps.Id <= 0)
                {
                    periodScheduleEntry.DateCreated = DateTime.Now; //TODO: Fix date created logic, setting to null whne ps.Id > 0
                }
                
                periodSchedule.Add(periodScheduleEntry);
            }
            
            await _context.BulkInsertOrUpdateAsync(periodSchedule, new BulkConfig
            {
                PreserveInsertOrder = false,
                SetOutputIdentity = true
            });

        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue updating period in the database", ex);
        }
    }

    public async Task BulkDeletePeriodScheduleEntriesAsync(List<int> periodScheduleEntriesToDelete)
    {
        try
        {
            var periodSchedules = new List<PeriodSchedule>();
            
            foreach (var id in periodScheduleEntriesToDelete)
            {
                periodSchedules.Add(new PeriodSchedule
                {
                    Id = id
                });
            }

            await _context.BulkDeleteAsync(periodSchedules);
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue deleting period schedule entries from the database", ex);
        }
    }

    public async Task<List<PeriodEntry>> GetPeriodsByClassroomIdAsync(int classroomId, int cycleId)
    {
        try
        {
            var periodsList = new List<PeriodEntry>();

            var periods = await (from cs in _context.ClassroomSchedules
                join p in _context.Periods.Include(p => p.Teacher).Include(p => p.Course) on cs.PeriodId equals p
                    .PeriodId
                where cs.ClassroomId == classroomId && cs.Classroom.CycleId == cycleId && p.CycleId == cycleId
                select p).ToListAsync();



            foreach (var period in periods)
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
                    PeriodSchedule = period.PeriodSchedules
                        .Select(ps => new PeriodScheduleEntry
                        {
                            Id = ps.Id,
                            DayOfWeek = ps.DayOfWeek,
                            StartTime = ps.StartTime,
                            EndTime = ps.EndTime,
                        }).ToList(),
                    DateCreated = period.DateCreated,
                    DateUpdated = period.DateModified,
                });
            }
            
            return periodsList;
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("An error occurred while retrieving periods for the classroom.", ex);
        }
    }
}