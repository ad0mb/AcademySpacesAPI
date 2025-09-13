using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class Faculty
{
    public int FacultyId { get; set; }

    public int SchoolId { get; set; }

    public string? IdentityId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string Email { get; set; } = null!;

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual ICollection<Announcment> Announcments { get; set; } = new List<Announcment>();

    public virtual ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();

    public virtual ICollection<FacultyRole> FacultyRoles { get; set; } = new List<FacultyRole>();

    public virtual ICollection<Period> Periods { get; set; } = new List<Period>();

    public virtual School School { get; set; } = null!;
}
