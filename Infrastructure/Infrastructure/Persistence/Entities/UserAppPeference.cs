using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class UserAppPeference
{
    public string IdentityId { get; set; } = null!;

    public string PageBrightness { get; set; } = null!;

    public string Locale { get; set; } = null!;

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }
}
