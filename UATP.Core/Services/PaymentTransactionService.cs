using UATP.Core.ApiModels;
using UATP.Core.Interfaces;
using UATP.Core.Models;

namespace UATP.Core.Services;

public class PaymentTransactionService : IPaymentTransactionService
{
    private readonly IPaymentTransactionRepository _repository;
    private readonly IPaymentProviderRepository _paymentProviderRepository;
    private readonly ICurrencyRepository _currencyRepository;

    public PaymentTransactionService(IPaymentTransactionRepository repository, 
        IPaymentProviderRepository paymentProviderRepository, ICurrencyRepository currencyRepository)
    {
        _repository = repository;
        _paymentProviderRepository = paymentProviderRepository;
        _currencyRepository = currencyRepository;
    }
    public async Task<PaymentTransactionModel?> Add(PaymentTransactionModel model)
    {
        var result = await _repository.Add(model.ToDomainModel());

        return result.ToApiModel();
    }

    public async Task<PaymentTransactionModel?> Add(string paymentProvider, PaymentTransactionModel model)
    {
        var provider = await _paymentProviderRepository.GetPaymentProvider(paymentProvider);
        
        if(provider == null)
            throw new ArgumentException($"Payment provider: {provider} not found.");
        
        model.PaymentProviderId = provider.Id;
        
        var currency = await _currencyRepository.GetCurrency(model.Currency.ToUpper());
        
        if(currency == null)
            throw new ArgumentException($"Currency: {model.Currency.ToUpper()} not found.");

        var result =  await _repository.Add(model.ToDomainModel());
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