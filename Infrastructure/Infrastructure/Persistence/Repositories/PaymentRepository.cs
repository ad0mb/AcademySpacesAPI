using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class PaymentRepository: IPaymentRepository
{
    private readonly MyDbContext _context;
    
    public PaymentRepository(MyDbContext context)
    {
        _context = context;
        
    }

    public async Task<IEnumerable<PaymentEntity>> GetPaymentsAsync()
    {
        var payments = await _context.Payments.ToListAsync();

        return payments.Select(p => new PaymentEntity
        {
            PaymentId= p.Id,
            SchoolId = p.SchoolId,
            StudentId = p.StudentId,
            StudentFirstName = p.StudentFirstName,
            StudentLastName = p.StudentLastName,
            Description = p.Description,
            Amount = p.Amount,
            Date = p.Date,
            recipt_number = p.ReceiptNumber,
            ProssesedBy = p.ProssesedBy,
            
            
        });
    }

    public async Task AddPaymentAsync(PaymentEntity payment)
    {

        

            var newpayment = new Payment
            {
                Id = payment.PaymentId,
                SchoolId = payment.SchoolId,
                StudentId = payment.StudentId,
                StudentFirstName = payment.StudentFirstName,
                StudentLastName = payment.StudentLastName,
                Description = payment.Description,
                Amount = payment.Amount,
                Date = payment.Date,
                ReceiptNumber = payment.recipt_number,
                Status = "paid",
                ProssesedBy = payment.ProssesedBy,
            };
            
            await _context.Payments.AddAsync(newpayment);
            await _context.SaveChangesAsync();


        
      
        


    }
}