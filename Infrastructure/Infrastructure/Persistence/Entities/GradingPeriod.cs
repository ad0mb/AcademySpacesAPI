using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class GradingPeriod
{
    public int GradingPeriodId { get; set; }

    public int CycleId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public virtual Cycle Cycle { get; set; } = null!;

    public virtual ICollection<Period> Classes { get; set; } = new List<Period>();
}
