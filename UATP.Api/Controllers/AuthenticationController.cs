using Microsoft.AspNetCore.Mvc;
using UATP.Core.ApiModels;
using UATP.Core.Interfaces;

namespace UATP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    public IActionResult Post([FromBody] AuthenticationModel model)
    {
        return Ok(_authenticationService.Authenticate(model));
    }
}