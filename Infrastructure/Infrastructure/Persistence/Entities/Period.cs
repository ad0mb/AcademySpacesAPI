using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class Period
{
    public int PeriodId { get; set; }

    public int CycleId { get; set; }

    public int? TeacherId { get; set; }

    public int CourseId { get; set; }

    public string Name { get; set; } = null!;

    public string? Location { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual ICollection<ClassGradingPeriod> ClassGradingPeriods { get; set; } = new List<ClassGradingPeriod>();

    public virtual ClassroomSchedule? ClassroomSchedule { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Cycle Cycle { get; set; } = null!;

    public virtual ICollection<PeriodSchedule> PeriodSchedules { get; set; } = new List<PeriodSchedule>();

    public virtual Faculty? Teacher { get; set; }
}
