using System.ComponentModel.DataAnnotations;
using UATP.Core.Enums;

namespace UATP.Core.ApiModels;

public class FilterOptionsModel
{
    public string? ProviderName { get; set; }
    public TransactionStatus? Status { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}