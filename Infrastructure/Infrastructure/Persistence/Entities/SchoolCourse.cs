using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class SchoolCourse
{
    public int CourseId { get; set; }

    public int SchoolId { get; set; }

    public int CourseName { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

   // public virtual School School { get; set; } = null!;
}
