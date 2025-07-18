using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using EFCore.BulkExtensions;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class YearLevelRepository : IYearLevelRepository
{

    private readonly MyDbContext _context;
    
    public YearLevelRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<List<YearLevelEntry>> GetYearLevelsBySchoolIdAsync(int schoolId)
    {
        try
        {
            var yearLevels = new List<YearLevelEntry>();
            
            var dbYearLevels = await (from yl in _context.YearLevels
                where yl.SchoolId == schoolId
                orderby yl.HierarchalLevel ascending 
                select yl).ToListAsync();

            foreach (var yearLevel in dbYearLevels)
            {
                yearLevels.Add(new YearLevelEntry
                {
                    Id = yearLevel.Id,
                    YearLevelName = yearLevel.YearLevelName,
                    YearLevelCode = yearLevel.YearLevelCode,
                    Description = yearLevel.Description,
                    DateCreated = yearLevel.DateCreated,
                    DateModified = yearLevel.DateModified,
                });
            }

            return yearLevels;
        }
        catch (DbUpdateException ex)
        {
            throw new DbException(ex.Message, ex);
        }
    }

    public async Task BulkUpdateOrInsertYearLevelHeirarchyAsync(List<YearLevelEntry> request)
    {
        try
        {
            var yearLevels = new List<YearLevel>();

            var count = 0;
                
            foreach (var yearLevel in request)
            {
                yearLevels.Add(new YearLevel
                {
                    Id = yearLevel.Id,
                    SchoolId = yearLevel.SchoolId,
                    HierarchalLevel = count,
                    YearLevelName = yearLevel.YearLevelName,
                    YearLevelCode = yearLevel.YearLevelCode,
                    Description = yearLevel.Description,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now
                });
                count++;
            }

            await _context.BulkInsertOrUpdateAsync(yearLevels, new BulkConfig
            {
                PreserveInsertOrder = false,
                SetOutputIdentity = true,
            });
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue updating or inserting year level hierarchy", ex);
        }
    }

    public async Task DeleteYearLevelsAsync(List<int> request)
    {
        try
        {
            var yearLevels = new List<YearLevel>();

            foreach (var yearLevelId in request)
            {
                yearLevels.Add(new YearLevel
                {
                    Id = yearLevelId
                });
            }

            await _context.BulkDeleteAsync(yearLevels);
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue deleting year levels", ex);
        }
    }
}