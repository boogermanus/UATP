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
}