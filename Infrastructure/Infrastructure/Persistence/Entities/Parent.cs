using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class Parent
{
    public int ParentId { get; set; }

    public int SchoolId { get; set; }

    public int? RoleId { get; set; }

    public string? IdentityId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual Role? Role { get; set; }

    public virtual School School { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
