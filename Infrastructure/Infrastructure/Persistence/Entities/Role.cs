namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class Role
{
    public int RoleId { get; set; }

    public int SchoolId { get; set; }

    public string RoleName { get; set; } = null!;

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual ICollection<FacultyRole> FacultyRoles { get; set; } = new List<FacultyRole>();

    public virtual ICollection<Parent> Parents { get; set; } = new List<Parent>();

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    public virtual School School { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
