namespace brandportal_dotnet.Contracts.Client.Profile;

public class TransactionDto
{
    public string Id { get; set; }
    public string TransactionName { get; set; }
    public decimal? Points { get; set; }
    public string TransactionType { get; set; }
    public DateTime? TransactionTime { get; set; }
}

public class StatisticsTransactionDto
{
    public decimal? TotalPoints { get; set; }
    public decimal? TotalCreatePlans { get; set; }
    public decimal? TotalPlace { get; set; }
    public decimal? TotalExpenses { get; set; }
}

public class StatisticsTransactionResponse
{
    public StatisticsTransactionDto StatisticsTransaction { get; set; }
    public List<TransactionDto> Transactions { get; set; }
}