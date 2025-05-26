namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class FacultyRole
{
    public int FacultyId { get; set; }

    public int RoleId { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual Faculty Faculty { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
