using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using EFCore.BulkExtensions;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
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

    public async Task SetStudentsGradesAsync(int schoolId, int cycleId, int periodId, List<StudentGradeEntry> gradeEntries)
    {
        try
        {
            //TODO: Validate that the assignments belong to the cycle and school

            var entriesFromDb = await (from sg in _context.StudentGrades
                where sg.Assignment.PeriodId == periodId &&
                      sg.Assignment.Period.Period.CycleId == cycleId &&
                      sg.Student.SchoolId == schoolId
                select sg).ToListAsync();
            
            var entriesToDelete = entriesFromDb.Where(sg => !gradeEntries.Any(g => g.AssignmentId == sg.AssignmentId && g.StudentId == sg.StudentId)).ToList();
            
            await _context.BulkDeleteAsync(entriesToDelete);
            
            var grades = new List<StudentGrade>();
            
            foreach (var gradeEntry in gradeEntries)
            {
                grades.Add(new StudentGrade
                {
                    AssignmentId = gradeEntry.AssignmentId,
                    StudentId = gradeEntry.StudentId,
                    Score = gradeEntry.Score,
                });
            }

            await _context.BulkInsertOrUpdateAsync(grades, new BulkConfig
            {
                PreserveInsertOrder = false,
                SetOutputIdentity = true,
            });
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Database update error occurred.", ex);
        }
    }
}