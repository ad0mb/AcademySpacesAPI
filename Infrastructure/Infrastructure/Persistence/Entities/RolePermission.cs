namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class RolePermission
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string PermissionName { get; set; } = null!;

    public bool Create { get; set; }

    public bool Delete { get; set; }

    public bool Update { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual Role Role { get; set; } = null!;
}
