using FakeItEasy;
using UATP.Core.ApiModels;
using UATP.Core.Interfaces;
using UATP.Core.Models;
using UATP.Core.Services;

namespace UATP.Tests.Services;

[TestFixture]
public class PaymentTransactionServiceTests
{
    private IPaymentTransactionService _service;
    private IPaymentTransactionRepository _repository;
    private IPaymentProviderRepository _paymentProviderRepository;
    private ICurrencyRepository _currencyRepository;
    [SetUp]
    public void SetUp()
    {
        _repository = A.Fake<IPaymentTransactionRepository>();
        _paymentProviderRepository = A.Fake<IPaymentProviderRepository>();
        _currencyRepository = A.Fake<ICurrencyRepository>();

        A.CallTo(() => _paymentProviderRepository.GetPaymentProvider("paypal")).Returns(
            new PaymentProvider { Id = 1, Name = "paypal", NormalizedName = "PayPal" });
        A.CallTo(() => _currencyRepository.GetCurrency("USD"))
            .Returns(new Currency { Id = 1, Code = "USD", Symbol = "$", Name = "US Dollar"});
        
        _service = new PaymentTransactionService(_repository,  _paymentProviderRepository, _currencyRepository);
    }

    [TearDown]
    public void TearDown()
    {
        _service = null;
    }
    
    [Test]
    public void AddShouldNotThrow()
    {
        
        Assert.That(() => _service.Add("paypal", new PaymentTransactionModel()), Throws.Nothing);
    }

    [Test]
    public void AddShouldThrowIfPaymentProviderDoesNotExist()
    {
        PaymentProvider? provider = null;
        A.CallTo(() => _paymentProviderRepository.GetPaymentProvider("test")).Returns(provider);
        Assert.That(() => _service.Add("test", new PaymentTransactionModel()), Throws.ArgumentException);
    }

    [Test]
    public void AddShouldCallPaymentProviderRepositoryGet()
    {
        _service.Add("paypal", new PaymentTransactionModel());
        A.CallTo(() => _paymentProviderRepository.GetPaymentProvider("paypal")).MustHaveHappened();
    }

    [Test]
    public void AddShouldCallCurrencyRepositoryGet()
    {
        _service.Add("paypal", new PaymentTransactionModel());
        A.CallTo(() => _currencyRepository.GetCurrency("USD")).MustHaveHappened();
    }

    [Test]
    public void AddShouldThrowIfCurrencyDoesNotExist()
    {
        Currency? currency = null;
        A.CallTo(() => _currencyRepository.GetCurrency("USD")).Returns(currency);
        Assert.That(() => _service.Add("paypal", new PaymentTransactionModel()), Throws.ArgumentException);
    }

    [Test]
    public void AddShouldCallRepositoryAdd()
    {
        _service.Add("paypal", new PaymentTransactionModel());
        
        A.CallTo(() => _repository.Add(A<PaymentTransaction>._)).MustHaveHappened();
    }

    [Test]
    public void GetShouldNotThrow()
    {
        Assert.That(() => _service.Get(new FilterOptionsModel()), Throws.Nothing);
    }

    [Test]
    public void GetShouldCallRepositoryGet()
    {
        _service.Get(new FilterOptionsModel());
        A.CallTo(() => _repository.Get(A<FilterOptionsModel>._)).MustHaveHappened();
    }

    [Test]
    public void SummaryShouldNotThrow()
    {
        Assert.That(() => _service.Summary(), Throws.Nothing);
    }

    [Test]
    public async Task SummaryShouldCallGetPaymentTransactionCount()
    {
        await _service.Summary();
        A.CallTo(() => _repository.GetPaymentTransactionCount()).MustHaveHappened();
    }

    [Test]
    public async Task SummaryShouldReturnObjectWithTransactionCount()
    {
        A.CallTo(() => _repository.GetPaymentTransactionCount()).Returns(10);
        
        var result = await _service.Summary();
        
        Assert.That(result.TransactionsCount, Is.EqualTo(10));
    }

    [Test]
    public async Task SummaryShouldCallGetProviders()
    {
        await _service.Summary();
        A.CallTo(() => _paymentProviderRepository.GetPaymentProviders()).MustHaveHappened();
    }

    [Test]
    public async Task SummaryShouldCallGetProviderVolumeAtLeastOnce()
    {
        A.CallTo(() => _paymentProviderRepository.GetPaymentProviders())
            .Returns(new List<PaymentProvider>
            {
                new PaymentProvider { Id = 1, Name = "paypal", NormalizedName = "PayPal" },
                new PaymentProvider { Id = 2, Name = "trustly", NormalizedName = "Trustly" }
            });
        
        await _service.Summary();

        A.CallTo(() => _repository.GetProviderVolume(A<int>._)).MustHaveHappenedOnceOrMore();
    }

    [Test]
    public async Task SummaryShouldCallGetStatusCounts()
    {
        await _service.Summary();
        
        A.CallTo(() => _repository.GetStatusCounts()).MustHaveHappened();
    }
}