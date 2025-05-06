using System.ComponentModel.DataAnnotations;
using UATP.Core.Enums;

namespace UATP.Core.ApiModels;

public class PaymentTransactionModel
{
    [MaxLength(20)]
    public required string ProviderName  { get; set; }
    public required decimal Amount { get; set; }
    // I am making the assumption here that we will store the currency code
    [MaxLength(3)]
    public required string Currency { get; set; }
    public required TransactionStatus Status { get; set; }
    public required DateTime Timestamp { get; set; }
    public required string PayerEmail  { get; set; }
    [MaxLength(20)]
    public required string PaymentMethod  { get; set; }
}