using System;
using System.Collections.Generic;

namespace AcademySpacesAPI.Infrastructure.Persistence.Entities;

public partial class Organization
{
    public int OrganizationId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual ICollection<School> Schools { get; set; } = new List<School>();
}
