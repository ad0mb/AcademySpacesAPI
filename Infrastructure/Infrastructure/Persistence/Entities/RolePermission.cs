using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class RolePermission
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string PermissionName { get; set; } = null!;

    public bool CanCreate { get; set; }

    public bool CanDelete { get; set; }

    public bool CanUpdate { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual Role Role { get; set; } = null!;
}
