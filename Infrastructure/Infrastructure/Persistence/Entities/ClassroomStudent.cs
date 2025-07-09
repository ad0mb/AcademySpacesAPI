using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class ClassroomStudent
{
    public int ClassroomId { get; set; }

    public int StudentId { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateUpdated { get; set; }

    public virtual Classroom Classroom { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
