namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class Faculty
{
    public int FacultyId { get; set; }

    public int SchoolId { get; set; }

    public string? IdentityId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string Email { get; set; } = null!;

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();

    public virtual ICollection<FacultyRole> FacultyRoles { get; set; } = new List<FacultyRole>();

    public virtual School School { get; set; } = null!;
}
