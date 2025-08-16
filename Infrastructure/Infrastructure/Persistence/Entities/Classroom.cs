using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class Classroom
{
    public int ClassroomId { get; set; }

    public int CycleId { get; set; }

    public int? ClassroomTeacherId { get; set; }

    public string ClassroomName { get; set; } = null!;

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual ICollection<ClassroomStudent> ClassroomStudents { get; set; } = new List<ClassroomStudent>();

    public virtual Faculty? ClassroomTeacher { get; set; }

    public virtual Cycle Cycle { get; set; } = null!;

    public virtual ICollection<Period> Periods { get; set; } = new List<Period>();
}
