using System.ComponentModel.DataAnnotations;

namespace UATP.Tests.Models;

public class ModelTestBase
{
    protected ValidationContext GetValidationContext(object model)
    {
        return new ValidationContext(model, null, null);
    }
}