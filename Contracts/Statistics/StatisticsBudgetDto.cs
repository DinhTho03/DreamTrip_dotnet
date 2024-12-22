namespace brandportal_dotnet.Contracts.Statistics;

public class StatisticsBudgetDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    
    public decimal? Amount { get; set; }
    public string TransactionId { get; set; }
    public string BankCode { get; set; } = string.Empty;
    
    public string OrderInfo { get; set; } = string.Empty;
    public DateTime? PaymentDate { get; set; }
    
    public string CardType { get; set; } = string.Empty;
    
    public string TransactionStatus { get; set; } = string.Empty;
    
    public string SecureHash { get; set; } = string.Empty;
    public int? Point { get; set; } 
}

public class StatisticsBudgetCard
{
    public decimal? TotalAmount { get; set; } = 0;
    public int? TotalPoint { get; set; } = 0;
    public int TotalTransaction { get; set; }
}