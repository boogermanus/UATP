using UATP.Core.ApiModels;
using UATP.Core.Enums;
using UATP.Core.Models;

namespace UATP.Core.Interfaces;
public interface IPaymentTransactionRepository
{
    Task<PaymentTransaction> Add(PaymentTransaction paymentTransaction);
    Task<IEnumerable<PaymentTransaction>> Get(FilterOptionsModel options);
    Task<int> GetPaymentTransactionCount();
    Task<decimal> GetProviderVolume(int providerId);
    Task<Tuple<int, int, int>> GetStatusCounts();
}