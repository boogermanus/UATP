using Microsoft.EntityFrameworkCore;
using UATP.Core.Models;


public class AppDbContext : DbContext
{
    public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {}
}