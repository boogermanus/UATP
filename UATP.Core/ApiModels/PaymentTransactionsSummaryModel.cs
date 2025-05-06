namespace UATP.Core.ApiModels;

public class PaymentTransactionsSummaryModel
{
    public int TransactionsCount { get; set; }
    public Dictionary<string, decimal> ProviderVolumes { get; set; } = new Dictionary<string, decimal>();
    public int PendingCount { get; set; }
    public int CompletedCount { get; set; }
    public int FailedCount { get; set; }
}