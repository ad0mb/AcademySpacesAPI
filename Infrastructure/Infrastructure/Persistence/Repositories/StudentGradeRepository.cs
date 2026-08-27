using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
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

    public async Task CreateStudentGradesAsync(int assignmentId, int periodId, List<StudentGradeEntry> entries)
    {
        try
        {
            var assignment = await (from a in _context.Assignments
                where a.AssignmentId == assignmentId && a.Period.PeriodId == periodId
                select a).FirstOrDefaultAsync();

            if (assignment == null)
            {
                throw new NotFoundException("Assignment not found");
            }

            foreach (var entry in entries)
            {
                if (entry.Score < 0 || entry.Score > assignment.MaxScore)
                {
                    throw new InvalidScoreException($"Score for student {entry.StudentId} is out of range.");
                }
            }

            foreach (var entry in entries)
            {
                await _context.StudentGrades.AddAsync(new StudentGrade
                {
                    AssignmentId = assignmentId,
                    StudentId = entry.StudentId,
                    Score = entry.Score
                });
            }

            var result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                throw new NoRowsAffectedException("Grades not created");
            }
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue adding grades to the database", ex);
        }
    }

    public async Task UpdateStudentGradesAsync(int assignmentId, int periodId, List<StudentGradeEntry> entries)
    {
        try
        {
            var assignment = await (from a in _context.Assignments
                where a.AssignmentId == assignmentId && a.Period.PeriodId == periodId
                select a).FirstOrDefaultAsync();

            if (assignment == null)
            {
                throw new NotFoundException("Assignment not found");
            }

            foreach (var entry in entries)
            {
                if (entry.Score < 0 || entry.Score > assignment.MaxScore)
                {
                    throw new InvalidScoreException($"Score for student {entry.StudentId} is out of range.");
                }
            }

            foreach (var entry in entries)
            {
                var grade = await (from sg in _context.StudentGrades
                    where sg.AssignmentId == assignmentId && sg.StudentId == entry.StudentId
                    select sg).FirstOrDefaultAsync();

                if (grade == null)
                {
                    throw new NotFoundException($"Grade not found for student {entry.StudentId}");
                }

                grade.Score = entry.Score;
            }

            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue updating grades in the database", ex);
        }
    }

    public async Task DeleteStudentGradesAsync(int assignmentId, int periodId, List<int> studentIds)
    {
        try
        {
            var assignment = await (from a in _context.Assignments
                where a.AssignmentId == assignmentId && a.Period.PeriodId == periodId
                select a).FirstOrDefaultAsync();

            if (assignment == null)
            {
                throw new NotFoundException("Assignment not found");
            }

            var grades = await (from sg in _context.StudentGrades
                where sg.AssignmentId == assignmentId && studentIds.Contains(sg.StudentId)
                select sg).ToListAsync();

            if (grades.Count == 0)
            {
                throw new NotFoundException("No grades found");
            }

            _context.StudentGrades.RemoveRange(grades);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue deleting grades from the database", ex);
        }
    }
}