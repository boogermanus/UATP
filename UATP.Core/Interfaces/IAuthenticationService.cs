using UATP.Core.ApiModels;

namespace UATP.Core.Interfaces;

public interface IAuthenticationService
{
    string? Authenticate(AuthenticationModel model);
}