using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using UATP.Core.Enums;
using UATP.Core.Models;

namespace UATP.Core.ApiModels;

public class PaymentTransactionModel
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
            Currency = Currency,
            Status = Status,
            Timestamp = DateTime.UtcNow,
            PayerEmail = PayerEmail,
            PaymentMethod = PaymentMethod
        };
    }
}