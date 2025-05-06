using System.ComponentModel.DataAnnotations;
using UATP.Core.ApiModels;
using UATP.Core.Enums;

namespace UATP.Core.Models;

public class PaymentTransaction
{
    // I will use a GUID for this value, but SQLlite does not play nice with them
    // so we will use a string
    [Key]
    public required string TransactionId { get; set; }
    public required decimal Amount { get; set; }
    // I am making the assumption here that we will store the currency code
    public required int CurrencyId { get; set; }
    public Currency? Currency { get; set; }
    public required TransactionStatus Status { get; set; }
    public required DateTime Timestamp { get; set; }
    public required string PayerEmail  { get; set; }
    [MaxLength(20)]
    public required string PaymentMethod  { get; set; }
    public required int PaymentProviderId { get; set; }
    public PaymentProvider? PaymentProvider { get; set; }

    public PaymentTransactionModel ToApiModel()
    {
        return new PaymentTransactionModel
        {
            TransactionId = TransactionId,
            Amount = Amount,
            Status = Status,
            Timestamp = Timestamp,
            PayerEmail = PayerEmail,
            PaymentMethod = PaymentMethod
        };
    }
    
}