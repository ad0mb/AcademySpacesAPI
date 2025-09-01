using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class Assignment
{
    public int AssignmentId { get; set; }

    public int PeriodId { get; set; }

    public string AssignmentType { get; set; } = null!;

    public string AssignmentName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public virtual ClassroomSchedule Period { get; set; } = null!;
}
