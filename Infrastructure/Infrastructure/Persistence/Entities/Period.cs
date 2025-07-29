using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class Period
{
    public int PeriodId { get; set; }

    public int? TeacherId { get; set; }

    public int CourseId { get; set; }

    public string? Location { get; set; }

    public int Capacity { get; set; }

    public int? DayOfWeek { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Faculty? Teacher { get; set; }
}
