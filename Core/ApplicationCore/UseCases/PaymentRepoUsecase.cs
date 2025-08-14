using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class PaymentRepoUsecase
{
    private readonly IPaymentRepository _paymentRepository;
    
    public PaymentRepoUsecase(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }


    public async Task<IEnumerable<PaymentEntity>> GetAllpaymentsAsync()
    {
        Console.WriteLine("We are getting all payments in the usecase layer");
        return await _paymentRepository.GetPaymentsAsync();
    }

    public async Task AddPaymentAsync(PaymentEntity payment)
    {
        await _paymentRepository.AddPaymentAsync(payment);
    }
    
    
    
}