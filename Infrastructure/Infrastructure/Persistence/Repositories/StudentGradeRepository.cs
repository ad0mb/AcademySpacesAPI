using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using Infrastructure.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class StudentGradeRepository : IStudentGradeRepository
{
    
    private readonly MyDbContext _context;
    
    public StudentGradeRepository (MyDbContext context)
    {
        _context = context;
    }

    public async Task<List<StudentGradeEntry>> GetStudentsGradesAsync(int cycleId, int assignmentId, int periodId)
    {
        try
        {
            var studentGrades = new List<StudentGradeEntry>();
            
            var dbStudentGrades = await (from sg in _context.StudentGrades
                where 
                    sg.Assignment.Period.Period.CycleId == cycleId &&
                    sg.Assignment.Period.PeriodId == periodId
                    
                    && (assignmentId <= 0 || sg.AssignmentId == assignmentId)
                    
                select sg).ToListAsync();

            foreach (var studentGradeEntry in dbStudentGrades)
            {
                studentGrades.Add(new StudentGradeEntry
                {
                    AssignmentId = studentGradeEntry.AssignmentId,
                    StudentId = studentGradeEntry.StudentId,
                    Score = studentGradeEntry.Score,
                    DateCreated = studentGradeEntry.DateCreated,
                    DateUpdated = studentGradeEntry.DateModified
                });
            }
            
            return studentGrades;
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Database update error occurred.", ex);
        }
    }
}