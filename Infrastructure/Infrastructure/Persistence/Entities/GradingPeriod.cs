using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class GradingPeriod
{
    public int GradingPeriodId { get; set; }

    public int CycleId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public DateTime? DateModified { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual ICollection<ClassGradingPeriod> ClassGradingPeriods { get; set; } = new List<ClassGradingPeriod>();

    public virtual Cycle Cycle { get; set; } = null!;
}
