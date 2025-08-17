using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class ClassroomSchedule
{
    public int ClassroomId { get; set; }

    public int PeriodId { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual Classroom Classroom { get; set; } = null!;

    public virtual Period Period { get; set; } = null!;
}
