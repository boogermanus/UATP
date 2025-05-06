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
        Amount = decimal.Zero;
        Status = TransactionStatus.Completed;
        Timestamp = DateTime.UtcNow;
        PayerEmail = string.Empty;
        PaymentMethod = string.Empty;
    }

    public string TransactionId { get; set; } = string.Empty;
    public required decimal Amount { get; set; }
    public required TransactionStatus Status { get; set; }
    public required DateTime Timestamp { get; set; }
    [EmailAddress]
    public required string PayerEmail  { get; set; }
    [MaxLength(20)]
    public required string PaymentMethod  { get; set; }
    public string StatusString => Enum.GetName(typeof(TransactionStatus), Status) ?? string.Empty;
    public int PaymentProviderId { get; set; }
    public int CurrencyId { get; set; }

    public PaymentTransaction ToDomainModel()
    {
        return new PaymentTransaction
        {
            TransactionId = Guid.NewGuid().ToString(),
            Amount = Amount,
            Status = Status,
            Timestamp = DateTime.UtcNow,
            PayerEmail = PayerEmail,
            PaymentMethod = PaymentMethod.ToUpper(),
            PaymentProviderId = PaymentProviderId,
            CurrencyId = CurrencyId,
        };
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(Amount == decimal.Zero)
            yield return new ValidationResult("Amount cannot be 0.", [nameof(Amount)]);
    }
}