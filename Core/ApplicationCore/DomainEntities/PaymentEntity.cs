namespace Core.ApplicationCore.DomainEntities;

public class PaymentEntity
{
    public int PaymentId { get; set; }
    public int SchoolId { get; set; }
    public int? StudentId { get; set; }
    public string StudentFirstName { get; set; } = null!;

    public string StudentLastName { get; set; } = null!;
    public Decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public string recipt_number { get; set; }
    
    public int ProssesedBy { get; set; }
}