using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class Classroom
{
    public int ClassId { get; set; }

    public int SchoolId { get; set; }

    public int? ClassroomTeacherId { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual Faculty? ClassroomTeacher { get; set; }

    public virtual School School { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
