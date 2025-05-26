namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class School
{
    public int SchoolId { get; set; }

    public int? OrganizationId { get; set; }

    public string Name { get; set; } = null!;

    public string CountryOfOrigin { get; set; } = null!;

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();

    public virtual ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();

    public virtual Organization? Organization { get; set; }

    public virtual ICollection<Parent> Parents { get; set; } = new List<Parent>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
