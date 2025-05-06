using System.ComponentModel.DataAnnotations;
using UATP.Core.Enums;

namespace UATP.Core.ApiModels;

public class FilterOptionsModel : IValidatableObject
{
    public string? ProviderName { get; set; }
    public TransactionStatus? Status { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(From > To)
            yield return new ValidationResult("From must be less than To", [nameof(From), nameof(To)]);

        if (To < From)
            yield return new ValidationResult("To must be grater than From", [nameof(To), nameof(From)]);
    }
}