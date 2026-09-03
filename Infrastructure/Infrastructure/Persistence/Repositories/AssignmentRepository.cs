using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Enums;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
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
                where a.PeriodId == periodId && a.Period.CycleId == cycleId
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

    public async Task CreateAssignmentAsync(AssignmentEntry request)
    {
        try
        {
            var assignment = new Assignment
            {
                PeriodId = request.PeriodId,
                AssignmentType = request.AssignmentType.ToString(),
                AssignmentName = request.AssignmentName,
                MaxScore = request.MaxScore,
                Description = request.Description,
                DueDate = request.DueDate
            };

            await _context.Assignments.AddAsync(assignment);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                throw new NoRowsAffectedException("Assignment not created");
            }
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue adding assignment to the database", ex);
        }
    }

    public async Task UpdateAssignmentAsync(AssignmentEntry request)
    {
        try
        {
            var assignment = await (from a in _context.Assignments
                where a.AssignmentId == request.AssignmentId && a.PeriodId == request.PeriodId
                select a).FirstOrDefaultAsync();

            if (assignment == null)
            {
                throw new NotFoundException("Assignment not found");
            }

            assignment.AssignmentType = request.AssignmentType.ToString();
            assignment.AssignmentName = request.AssignmentName;
            assignment.MaxScore = request.MaxScore;
            assignment.Description = request.Description;
            assignment.DueDate = request.DueDate;

            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue updating assignment in the database", ex);
        }
    }

    public async Task DeleteAssignmentAsync(int assignmentId, int periodId)
    {
        try
        {
            var assignment = await (from a in _context.Assignments
                where a.AssignmentId == assignmentId && a.PeriodId == periodId
                select a).FirstOrDefaultAsync();

            if (assignment == null)
            {
                throw new NotFoundException("Assignment not found");
            }

            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue deleting assignment from the database", ex);
        }
    }
}