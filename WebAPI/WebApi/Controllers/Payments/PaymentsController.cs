using AcademySpacesAPI.WebApi.DTOs.Requests;
using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.UseCases;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AcademySpacesAPI.WebApi.Controllers.Payments;

[ApiController]
[Authorize (AuthenticationSchemes = "FirebaseAuthScheme")]
[EnableRateLimiting("fixed")] 
[Route("api/[controller]")]
public class PaymentsController:ControllerBase
{
    
    private readonly PaymentRepoUsecase _paymentRepoUsecase;

    public PaymentsController(PaymentRepoUsecase paymentRepo)
    {
        _paymentRepoUsecase = paymentRepo;
    }
    
    
    
    
    [HttpGet("get-payments")]
    public async Task<ActionResult> GetPayments()
    {
        var result =await _paymentRepoUsecase.GetAllpaymentsAsync();
        //TODO:Pass into a Data Table to filter out unnesesary Data like IDs 
        
        
        return Ok(result);
    }
    
    
    [HttpPost("add-payment")]
    public async Task<ActionResult> AddPayment(CreatePaymentDTO paymentdto)
    {
        try
        {
        var payment = new PaymentEntity
        {
            SchoolId = paymentdto.schoolId,
            StudentFirstName = paymentdto.StudentFirstName,
            StudentLastName = paymentdto.StudentLastName,
            Amount = paymentdto.amount,
            Description = paymentdto.description,
            Date = paymentdto.date,
            ProssesedBy = paymentdto.ProssesedBy,
            recipt_number = paymentdto.recipt_number,
        };

        
            await _paymentRepoUsecase.AddPaymentAsync(payment);
            return Ok(new
                {
                    success = true,
                    message = "Payment Added"
                }
            );
        }
        catch (Exception e)
        {
            return BadRequest(new
            {
                success = false,
                message = "Failed to add payment",
                Error = e.Message
            });
        }
        
    }
    
 
    [HttpPost("try-payment")]
    public void TryPayment(CreatePaymentDTO paymentdto)
    {
        Console.WriteLine("try-payment Was triggured");
        Console.WriteLine(paymentdto.StudentFirstName + " \n" + paymentdto.StudentLastName + "\n " + paymentdto.ProssesedBy + "\n " + paymentdto.recipt_number + "\n " + paymentdto.amount + "\n " + paymentdto.date + "\n " + paymentdto.ProssesedBy + " \n" + paymentdto.recipt_number + " \n" + paymentdto.amount + "\n " + paymentdto.date);
    }
    
    
    
}