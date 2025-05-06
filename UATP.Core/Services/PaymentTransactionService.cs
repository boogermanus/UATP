using UATP.Core.ApiModels;
using UATP.Core.Interfaces;

namespace UATP.Core.Services;

public class PaymentTransactionService : IPaymentTransactionService
{
    private readonly IPaymentTransactionRepository _repository;

    public PaymentTransactionService(IPaymentTransactionRepository repository)
    {
        _repository = repository;
    }
    public async Task<PaymentTransactionModel?> Add(PaymentTransactionModel model)
    {
        // do any validation

        var result = await _repository.Add(model.ToDomainModel());

        return result.ToApiModel();
    }

    public Task<IEnumerable<PaymentTransactionModel>> Get(FilterOptionsModel model)
    {
        throw new NotImplementedException();
    }
}