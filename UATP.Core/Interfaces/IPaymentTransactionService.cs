using UATP.Core.ApiModels;

namespace UATP.Core.Interfaces;

public interface IPaymentTransactionService
{
    Task<PaymentTransactionModel?> Add(PaymentTransactionModel model);
    Task<IEnumerable<PaymentTransactionModel>> Get(FilterOptionsModel model);
}