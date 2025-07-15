using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class School
{
    public int SchoolId { get; set; }

    public int? OrganizationId { get; set; }

    public string Name { get; set; } = null!;

    public string CountryOfOrigin { get; set; } = null!;

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual ICollection<Announcment> Announcments { get; set; } = new List<Announcment>();

    public virtual ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();

    public virtual ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();

    public virtual Organization? Organization { get; set; }

    public virtual ICollection<Parent> Parents { get; set; } = new List<Parent>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<SchoolConfiguration> SchoolConfigurations { get; set; } = new List<SchoolConfiguration>();

    public virtual ICollection<SchoolCourse> SchoolCourses { get; set; } = new List<SchoolCourse>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<YearLevel> YearLevels { get; set; } = new List<YearLevel>();
}
