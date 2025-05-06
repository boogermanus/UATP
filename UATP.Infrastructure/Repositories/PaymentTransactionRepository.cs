using Microsoft.EntityFrameworkCore;
using UATP.Core.ApiModels;
using UATP.Core.Enums;
using UATP.Core.Interfaces;
using UATP.Core.Models;

namespace UATP.Infrastructure.Repositories;

public class PaymentTransactionRepository : IPaymentTransactionRepository
{
    private AppDbContext _context;

    public PaymentTransactionRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<PaymentTransaction> Add(PaymentTransaction paymentTransaction)
    {
        await _context.PaymentTransactions.AddAsync(paymentTransaction);
        await _context.SaveChangesAsync();
        return paymentTransaction;
    }

    public async Task<IEnumerable<PaymentTransaction>> Get(FilterOptionsModel options)
    {
        var query = _context.PaymentTransactions.AsQueryable();
        query = Filter(query, options);
        return await query.ToListAsync();
    }

    public async Task<int> GetPaymentTransactionCount()
    {
        return await _context.PaymentTransactions.CountAsync();
    }

    public async Task<IEnumerable<string>> GetProviders()
    {
        throw new NotImplementedException();
    }

    public async Task<decimal> GetProviderVolume(string providerName)
    {
        throw new NotImplementedException();
    }

    public async Task<Tuple<int, int, int>> GetStatusCounts()
    {
        var pendingCount = await _context.PaymentTransactions.CountAsync(pt => pt.Status == TransactionStatus.Pending);
        var completedCount =
            await _context.PaymentTransactions.CountAsync(pt => pt.Status == TransactionStatus.Completed);
        var failedCount = await _context.PaymentTransactions.CountAsync(pt => pt.Status == TransactionStatus.Failed);
        
        return new (pendingCount, completedCount, failedCount);
    }

    private IQueryable<PaymentTransaction> Filter(IQueryable<PaymentTransaction> query, FilterOptionsModel options)
    {
        // if(!string.IsNullOrWhiteSpace(options.ProviderName))
        //     query = query.Where(p => p.ProviderName == options.ProviderName);
        
        if(options.Status != null)
            query = query.Where(p => p.Status == options.Status);
        
        if(options.From != null)
            query = query.Where(p => p.Timestamp >= options.From);
        
        if(options.To != null)
            query = query.Where(p => p.Timestamp <= options.To);

        return query;
    }
}