using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class ClassroomSchedule
{
    public int PeriodId { get; set; }

    public int ClassroomId { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual Classroom Classroom { get; set; } = null!;

    public virtual Period Period { get; set; } = null!;
}
