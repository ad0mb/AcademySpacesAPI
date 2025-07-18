using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class YearLevel
{
    public int Id { get; set; }

    public int SchoolId { get; set; }

    public int HierarchalLevel { get; set; }

    public string YearLevelName { get; set; } = null!;

    public string YearLevelCode { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual School School { get; set; } = null!;
}
