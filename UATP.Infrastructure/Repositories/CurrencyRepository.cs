using Microsoft.EntityFrameworkCore;
using UATP.Core.Interfaces;
using UATP.Core.Models;

namespace UATP.Infrastructure.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly AppDbContext _context;

    public CurrencyRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Currency?> GetCurrency(string code)
    {
        return await _context.Currencies.FirstOrDefaultAsync(c => c.Code == code);
    }
}