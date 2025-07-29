using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class Course
{
    public int CourseId { get; set; }

    public int SchoolId { get; set; }

    public string CourseName { get; set; } = null!;

    public string CourseCode { get; set; } = null!;

    public string? CourseDescription { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual ICollection<Period> Periods { get; set; } = new List<Period>();

    public virtual School School { get; set; } = null!;
}
