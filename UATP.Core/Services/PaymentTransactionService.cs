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
        var result = await _repository.Add(model.ToDomainModel());

        return result.ToApiModel();
    }

    public async Task<IEnumerable<PaymentTransactionModel>> Get(FilterOptionsModel model)
    {
        var results = await _repository.Get(model);
        return results.Select(r => r.ToApiModel()).ToList();
    }

    public async Task<PaymentTransactionsSummaryModel> Summary()
    {
        var transactionCount = await _repository.GetPaymentTransactionCount();
        var providers = await _repository.GetProviders();
        var providerVolumes = new Dictionary<string, decimal>();
        
        foreach (var provider in providers)
        {
            var volume = await _repository.GetProviderVolume(provider);
            providerVolumes.Add(provider, volume);
        }

        var statusCounts = await _repository.GetStatusCounts();
        

        return new PaymentTransactionsSummaryModel
        {
            TransactionsCount = transactionCount,
            ProviderVolumes = providerVolumes,
            PendingCount = statusCounts.Item1,
            CompletedCount = statusCounts.Item2,
            FailedCount = statusCounts.Item3,
        };
    }
}