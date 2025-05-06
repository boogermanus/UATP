using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using UATP.Core.Enums;
using UATP.Core.Models;

namespace UATP.Core.ApiModels;

public class PaymentTransactionModel : IValidatableObject
{
    [SetsRequiredMembers]
    public PaymentTransactionModel()
    {
        ProviderName = string.Empty;
        Amount = decimal.Zero;
        Currency = "USD";
        Status = TransactionStatus.Completed;
        Timestamp = DateTime.UtcNow;
        PayerEmail = string.Empty;
        PaymentMethod = string.Empty;
    }

    public string TransactionId { get; set; } = string.Empty;
    [MaxLength(20)]
    public string ProviderName  { get; set; }
    public required decimal Amount { get; set; }
    // I am making the assumption here that we will store the currency code
    [MaxLength(3)]
    public required string Currency { get; set; }
    public required TransactionStatus Status { get; set; }
    public required DateTime Timestamp { get; set; }
    [EmailAddress]
    public required string PayerEmail  { get; set; }
    [MaxLength(20)]
    public required string PaymentMethod  { get; set; }
    public string StatusString => Enum.GetName(typeof(TransactionStatus), Status) ?? string.Empty;

    public PaymentTransaction ToDomainModel()
    {
        return new PaymentTransaction
        {
            TransactionId = Guid.NewGuid().ToString(),
            ProviderName = ProviderName,
            Amount = Amount,
            Currency = Currency.ToUpper(),
            Status = Status,
            Timestamp = DateTime.UtcNow,
            PayerEmail = PayerEmail,
            PaymentMethod = PaymentMethod
        };
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // for brevity, we'll only verify a handful of currency codes
        string[] validCurrencyCodes = ["USD", "EUR", "RUB", "JPY"];
        
        if(!validCurrencyCodes.Contains(Currency.ToUpper()))
            yield return new ValidationResult($"Invalid currency code {Currency}.", [nameof(Currency)]);
        if(Amount == decimal.Zero)
            yield return new ValidationResult("Amount cannot be 0.", [nameof(Amount)]);
    }
}