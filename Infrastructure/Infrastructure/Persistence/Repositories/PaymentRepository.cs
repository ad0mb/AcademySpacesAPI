using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

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
            Description = p.Description,
            Amount = p.Amount,
            Date = p.Date,
            recipt_number = p.ReceiptNumber,
            ProssesedBy = p.ProssesedBy,
            
            
        });
    }

    public async Task AddPaymentAsync(PaymentEntity payment)
    {

        try
        {

            var newpayment = new Payment
            {
                Id = payment.PaymentId,
                SchoolId = payment.SchoolId,
                StudentId = payment.StudentId,
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
        catch (DbUpdateException e)
        {
            Console.WriteLine("There was an Exception With the DB at PaymentRepository:" ,e);
        }



    }
}