using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class ClassGradingPeriod
{
    public int ClassId { get; set; }

    public int GradingPeriodId { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual Period Class { get; set; } = null!;

    public virtual GradingPeriod GradingPeriod { get; set; } = null!;
}
