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
            var result = await _service.Add(providerName, model);
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions([FromQuery] FilterOptionsModel filterOptions)
    {
        try
        {
            var result = await _service.Get(filterOptions);
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        try
        {
            return (Ok(await _service.Summary()));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}