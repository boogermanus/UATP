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
        var provider = await _paymentProviderRepository.GetPaymentProvider(paymentProvider.ToLower());
        
        if(provider == null)
            throw new ArgumentException($"Payment provider: {paymentProvider} not found.");
        
        model.PaymentProviderId = provider.Id;
        
        var currencyUpper = model.Currency.ToUpper();
        var currency = await _currencyRepository.GetCurrency(currencyUpper);

        if(currency == null)
            throw new ArgumentException($"Currency: {currencyUpper} not found.");
        
        model.CurrencyId = currency.Id;

        var result =  await _repository.Add(model.ToDomainModel());
        return result.ToApiModel();
    }

    public async Task<IEnumerable<PaymentTransactionModel>> Get(FilterOptionsModel model)
    {
        model.ProviderName = model.ProviderName?.ToLower();
        var results = await _repository.Get(model);
        return results.Select(r => r.ToApiModel()).ToList();
    }

    public async Task<PaymentTransactionsSummaryModel> Summary()
    {
        var transactionCount = await _repository.GetPaymentTransactionCount();
        var providers = await _paymentProviderRepository.GetPaymentProviders();
        var providerVolumes = new Dictionary<string, decimal>();
        
        foreach (var provider in providers)
        {
            var volume = await _repository.GetProviderVolume(provider.Id);
            providerVolumes.Add(provider.NormalizedName, volume);
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