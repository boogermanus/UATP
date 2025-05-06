using Microsoft.AspNetCore.Mvc;
using UATP.Core.ApiModels;
using UATP.Core.Interfaces;

namespace UATP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentTransactionsController : ControllerBase
{
    private readonly IPaymentTransactionService _service;
    
    public PaymentTransactionsController(IPaymentTransactionService service)
    {
        _service = service;
    }
    
    [HttpPost("ingest/{providerName}")]
    public async Task<IActionResult> IngestPaymentTransaction([FromRoute] string providerName,
        [FromBody] PaymentTransactionModel model)
    {
        try
        {
            model.ProviderName = providerName.ToLower();
            var result = await _service.Add(model);
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
}