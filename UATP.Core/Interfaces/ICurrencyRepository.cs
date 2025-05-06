using UATP.Core.Models;

namespace UATP.Core.Interfaces;

public interface ICurrencyRepository
{
    Task<Currency?> GetCurrency(string code);
}