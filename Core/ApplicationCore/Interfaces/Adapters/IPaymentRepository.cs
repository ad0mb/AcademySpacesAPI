using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IPaymentRepository
{
    public Task<IEnumerable<PaymentEntity>> GetPaymentsAsync();
    public Task AddPaymentAsync(PaymentEntity payment);
    
}