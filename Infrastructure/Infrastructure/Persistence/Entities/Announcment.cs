using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class Announcment
{
    public Guid Id { get; set; }

    public string Message { get; set; } = null!;

    public Guid? RecieverId { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid SenderId { get; set; }
}
