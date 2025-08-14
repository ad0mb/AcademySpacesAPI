using System;
using System.Collections.Generic;

namespace Infrastructure.Infrastructure.Persistence.Entities;

public partial class Payment
{
    public int Id { get; set; }

    public int SchoolId { get; set; }

    public int StudentId { get; set; }

    public string? Description { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public string ReceiptNumber { get; set; } = null!;

    public string? Paymentmethod { get; set; }

    public string? Feetype { get; set; }

    public string Status { get; set; } = null!;

    public int ProssesedBy { get; set; }

    public virtual School School { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
