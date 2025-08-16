using System.Runtime.InteropServices.JavaScript;
using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class SchoolRepository : ISchoolRepository
{
    
    private readonly MyDbContext _context;
    private readonly IRoleRepository _roleRepository;
    
    public SchoolRepository(MyDbContext context, IRoleRepository roleRepository)
    {
        _context = context;
        _roleRepository = roleRepository;
    }
    
    //TODO: Exception Handling (use result), return exception meant for core
    //Db update exception
    public async Task<int[]> CreateSchoolAsync(SchoolEntry request)
    {
        try
        {
            var school = new School
            {
                Name = request.SchoolName,
                CountryOfOrigin = request.SchoolCountry,
            };

            await _context.Schools.AddAsync(school);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                throw new NoRowsAffectedException("No rows were affected when creating the school.");
            }

            //TODO: Add permissions to add with each role later
            var chiefAdminRoleId = await _roleRepository.CreateRoleAsync(
                new RoleEntry { SchoolId = school.SchoolId, RoleName = "Chief Administrator" },
                ["chiefadministrator:000"]);
            await _roleRepository.CreateRoleAsync(
                new RoleEntry { SchoolId = school.SchoolId, RoleName = "Administrator" }, ["administrator:000"]);
            await _roleRepository.CreateRoleAsync(
                new RoleEntry { SchoolId = school.SchoolId, RoleName = "Teacher" }, []);
            await _roleRepository.CreateRoleAsync(
                new RoleEntry { SchoolId = school.SchoolId, RoleName = "Parent" }, []);
            await _roleRepository.CreateRoleAsync(
                new RoleEntry { SchoolId = school.SchoolId, RoleName = "Student" }, []);

            return [school.SchoolId, chiefAdminRoleId];
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue adding school to the database", ex);
        }
    }

    public async Task<SchoolConfigurationEntry> GetSchoolConfigurationAsync(int schoolId)
    {
        throw new NotImplementedException();
    }

    public async Task<(List<CycleEntry> cyclesList, int totalCount)> GetCyclesAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm)
    {
        try
        {
            var cycles = new List<CycleEntry>();

            IQueryable<Cycle> query = from c in _context.Cycles
                where c.SchoolId == schoolId
                orderby c.EndDate descending
                select c;

            var totalCount = await query.CountAsync();
            
            if (pageSize > 0 && pageNumber > 0)
            {
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            
            var dbCycles = await query
                .Include(c => c.GradingPeriods)
                .ToListAsync();

            foreach (var course in dbCycles)
            {
                var gradingPeriods = new List<GradingPeriodEntry>();
                foreach (var gradingPeriod in course.GradingPeriods.OrderBy(gp => gp.StartDate))
                {
                    gradingPeriods.Add(new GradingPeriodEntry
                    {
                        GradingPeriodId = gradingPeriod.GradingPeriodId,
                        StartDate = gradingPeriod.StartDate,
                        EndDate = gradingPeriod.EndDate,
                        DateCreated = gradingPeriod.DateCreated,
                        DateUpdated = gradingPeriod.DateModified
                    });
                }
                cycles.Add(new CycleEntry
                {
                    CycleId = course.CycleId,
                    SchoolId = course.SchoolId,
                    IsActive = course.IsActive,
                    isArchived = course.IsArchived,
                    CycleName = course.Name,
                    Code = course.Code,
                    ScheduleType = course.ScheduleType,
                    StartDate = course.StartDate,
                    EndDate = course.EndDate,
                    GradingPeriods = gradingPeriods,
                    IsExpired = DateOnly.FromDateTime(DateTime.Now) > course.EndDate,
                    DateCreated = course.DateCreated,
                    DateUpdated = course.DateModified
                });
            }
            
            return (cycles, totalCount);
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue retrieving cycles from the database", ex);
        }
    }
    
    public async Task<int?> GetActiveCycleIdAsync(int schoolId)
    {
        try
        {
            var cycleId = await (from c in _context.Cycles
                where c.SchoolId == schoolId && c.IsActive == true
                select c).SingleOrDefaultAsync();

            if (cycleId == null)
            {
                return null;
            }
            
            return cycleId.CycleId;
            
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue retrieving school configuration from the database", ex);
        }
    }
}