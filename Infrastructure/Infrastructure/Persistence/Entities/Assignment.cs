using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class Assignment
{
    public int AssignmentId { get; set; }

    public int PeriodId { get; set; }

    public string AssignmentType { get; set; } = null!;

    public string AssignmentName { get; set; } = null!;

    public int MaxScore { get; set; }

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual ClassroomSchedule Period { get; set; } = null!;

    public virtual ICollection<StudentGrade> StudentGrades { get; set; } = new List<StudentGrade>();
}
