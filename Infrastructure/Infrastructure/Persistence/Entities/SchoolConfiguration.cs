using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class SchoolConfiguration
{
    public int Id { get; set; }

    public int SchoolId { get; set; }

    public bool StaticClassroom { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual School School { get; set; } = null!;
}
