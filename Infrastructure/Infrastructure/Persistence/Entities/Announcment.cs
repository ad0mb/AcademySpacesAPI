using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class Announcment
{
    public int AnnouncmentId { get; set; }

    public string Message { get; set; } = null!;

    public int SenderId { get; set; }

    public int SchoolId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Priority { get; set; }

    public string? Category { get; set; }

    public string? AnnouncmentName { get; set; }

    public virtual School School { get; set; } = null!;

    public virtual Faculty Sender { get; set; } = null!;
}
