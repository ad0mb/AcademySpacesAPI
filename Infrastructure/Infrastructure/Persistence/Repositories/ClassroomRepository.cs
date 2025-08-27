using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using EFCore.BulkExtensions;
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
                CycleId = classroomEntry.CycleId,
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
    
    //TODO: Handle case if cycle is null or empty
    public async Task<(List<ClassroomEntry> classroomsList, int totalCount)> GetClassroomsAsync(int schoolId, int cycleId, int pageSize, int pageNumber, string? searchTerm)
    {
        try
        {
            List<ClassroomEntry> classrooms = new List<ClassroomEntry>();

            var query =  from c in _context.Classrooms
                from f in _context.Faculties.Where(f => f.FacultyId == c.ClassroomTeacherId).DefaultIfEmpty()
                join cs in _context.ClassroomStudents on c.ClassroomId equals cs.ClassroomId into studentGroup
                where c.Cycle.SchoolId == schoolId && c.CycleId == cycleId

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

    //TODO: Take a look at whether to keep delete logic local or make it a new method and change request type to include a delete flag
    public async Task UpdateClassroomScheduleAsync(int cycleId, int classroomId, List<int> periodId)
    {
        try
        {
            var result = await (from assignedScheduleEntries in _context.PeriodSchedules //conflict checker
                where assignedScheduleEntries.Period.CycleId == cycleId
                      && assignedScheduleEntries.Period.ClassroomSchedules.Any(cs => cs.ClassroomId == classroomId)
                from requestedScheduleEntries in _context.PeriodSchedules
                where requestedScheduleEntries.Period.CycleId == cycleId && periodId.Contains(requestedScheduleEntries
                                                                             .PeriodId)
                                                                         && requestedScheduleEntries.PeriodId !=
                                                                         assignedScheduleEntries.PeriodId
                                                                         && requestedScheduleEntries.DayOfWeek !=
                                                                         assignedScheduleEntries.DayOfWeek
                                                                         && requestedScheduleEntries.StartTime <
                                                                         assignedScheduleEntries.EndTime
                                                                         && requestedScheduleEntries.EndTime >
                                                                         assignedScheduleEntries.StartTime
                select new
                {
                    ClassPeriodId = assignedScheduleEntries.PeriodId,
                    ConflictingPeriodId = requestedScheduleEntries.PeriodId,
                    DayOfWeek = requestedScheduleEntries.DayOfWeek
                }).AnyAsync();

            if (result)
            {
                throw new SchedulingConflictException("One or more of the requested periods conflict with existing scheduled periods for this classroom.");
            }
            
            
            var dbDeleteEntries = new List<ClassroomSchedule>();
            var dbAddOrUpdateEntries = new List<ClassroomSchedule>();

            var entriesToDelete = await (from c in _context.ClassroomSchedules
                where c.ClassroomId == classroomId && !periodId.Contains(c.PeriodId)
                select c.PeriodId).ToListAsync();

            foreach (var entry in entriesToDelete)
            {
                dbDeleteEntries.Add(new ClassroomSchedule
                {
                    ClassroomId = classroomId,
                    PeriodId = entry
                });
            }

            await _context.BulkDeleteAsync(dbDeleteEntries);
            
            foreach (var id in periodId)
            {
                dbAddOrUpdateEntries.Add(new ClassroomSchedule
                {
                    ClassroomId = classroomId,
                    PeriodId = id
                });
            }

            //TODO: Does not add date created and modified to bulk inserted or updated entries
            await _context.BulkInsertOrUpdateAsync(dbAddOrUpdateEntries);
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue updating classroom schedule in the database", ex);
        }
    }
}