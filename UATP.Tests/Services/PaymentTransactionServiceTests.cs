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
    [SetUp]
    public void SetUp()
    {
        _repository = A.Fake<IPaymentTransactionRepository>();
        _service = new PaymentTransactionService(_repository);
    }

    [TearDown]
    public void TearDown()
    {
        _service = null;
    }

    [Test]
    public void AddShouldNotThrow()
    {
        Assert.That(() => _service.Add(new PaymentTransactionModel()), Throws.Nothing);
    }

    [Test]
    public void AddShouldCallRepositoryAdd()
    {
        _service.Add(new PaymentTransactionModel());
        
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
        A.CallTo(() => _repository.GetProviders()).MustHaveHappened();
    }

    [Test]
    public async Task SummaryShouldCallGetProviderVolumeAtLeastOnce()
    {
        A.CallTo(() => _repository.GetProviders()).Returns(new List<string> {"paypal","test"});
        
        await _service.Summary();

        A.CallTo(() => _repository.GetProviderVolume(A<string>._)).MustHaveHappenedOnceOrMore();
    }

    [Test]
    public async Task SummaryShouldCallGetStatusCounts()
    {
        await _service.Summary();
        
        A.CallTo(() => _repository.GetStatusCounts()).MustHaveHappened();
    }
}