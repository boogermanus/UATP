using UATP.Core.Models;

namespace UATP.Core.Interfaces;

public interface IPaymentProviderRepository
{
    Task<PaymentProvider?> GetPaymentProvider(string name);
}