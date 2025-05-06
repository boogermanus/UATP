using Microsoft.EntityFrameworkCore;
using UATP.Core.Interfaces;
using UATP.Core.Models;

namespace UATP.Infrastructure.Repositories;

public class PaymentProviderRepository : IPaymentProviderRepository
{
    private readonly AppDbContext _context;

    public PaymentProviderRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<PaymentProvider?> GetPaymentProvider(string name)
    {
        return await _context.PaymentProviders.FirstOrDefaultAsync(pp => pp.Name == name);
    }

    public async Task<IEnumerable<PaymentProvider>> GetPaymentProviders()
    {
        return await _context.PaymentProviders.ToListAsync();
    }
}