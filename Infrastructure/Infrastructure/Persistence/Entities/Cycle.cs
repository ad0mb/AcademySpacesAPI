using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class Cycle
{
    public int CycleId { get; set; }

    public int SchoolId { get; set; }

    public bool IsActive { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public virtual ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();

    public virtual ICollection<GradingPeriod> GradingPeriods { get; set; } = new List<GradingPeriod>();

    public virtual School School { get; set; } = null!;
}
