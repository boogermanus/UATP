using UATP.Core.ApiModels;
using UATP.Core.Enums;

namespace UATP.Tests.Models;

[TestFixture]
public class PaymentTransactionModelTests : ModelTestBase
{
    [Test]
    public void ToDomainModelShouldSetTransactionId()
    {
        var model = new PaymentTransactionModel();
        
        var result = model.ToDomainModel();

        Assert.That(result.TransactionId, Is.Not.Empty);
    }

    [Test]
    public void ToDomainModelShouldSetTransactionIdToGuidString()
    {
        var model = new PaymentTransactionModel();
        
        var result = model.ToDomainModel();

        Assert.That(Guid.Parse(result.TransactionId), Is.AssignableTo<Guid>());
    }
    
    [Test]
    public void ToDomainModelShouldSetPaymentMethodToUpperCase()
    {
        var model = new PaymentTransactionModel
        {
            PaymentMethod = "CreditCard"
        };

        var result = model.ToDomainModel();
        
        Assert.That(result.PaymentMethod, Is.EqualTo(model.PaymentMethod.ToUpper()));
    }

    [Test]
    public void ValidateShouldReturnZeroErrorForAmount()
    {
        var model = new PaymentTransactionModel
        {
            Amount = 0,
            PaymentMethod = "CreditCard"
        };
        
        var results = model.Validate(GetValidationContext(model));
        
        Assert.That(results.First().ErrorMessage, Contains.Substring("Amount"));
    }

    [Test]
    public void ValidateShouldReturnPaymentMethodError()
    {
        var model = new PaymentTransactionModel
        {
            PaymentMethod = "test"
        };
        
        var results = model.Validate(GetValidationContext(model));
        
        Assert.That(results.First().ErrorMessage, Contains.Substring("PaymentMethod"));
    }

    [Test]
    public void ValidateShouldReturnErrorForStatusValue()
    {
        var model = new PaymentTransactionModel
        {
            Amount = 10,
            PaymentMethod = "CreditCard",
            Status = (TransactionStatus)3
        };
        
        var results = model.Validate(GetValidationContext(model));
        
        Assert.That(results.First().ErrorMessage, Contains.Substring("Status"));
    }
}