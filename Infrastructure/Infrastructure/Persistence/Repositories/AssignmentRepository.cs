using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Enums;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using Infrastructure.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    
    private readonly MyDbContext _context;

    public AssignmentRepository(MyDbContext context)
    {
        _context = context;
    }


    public async Task<List<AssignmentEntry>> GetAssignmentsByPeriodIdAsync(int cycleId, int periodId)
    {
        try
        {
            var assignments = new List<AssignmentEntry>();
            
            var dbAssignments = await (from a in _context.Assignments
                where a.PeriodId == periodId && a.Period.Period.CycleId == cycleId
                select a).ToListAsync();

            foreach (var assignment in dbAssignments)
            {
                assignments.Add(new AssignmentEntry
                {
                    AssignmentId = assignment.AssignmentId,
                    AssignmentType = (AssignmentType)Enum.Parse(typeof(AssignmentType), assignment.AssignmentType, true),
                    AssignmentName = assignment.AssignmentName,
                    MaxScore = assignment.MaxScore,
                    Description = assignment.Description,
                    DueDate = assignment.DueDate
                });
            }

            return assignments;
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Database update error occurred while retrieving assignments.", ex);
        }
    }
}