namespace brandportal_dotnet.Contracts.Statistics;

public class StatisticsPublicPlanStatusDto
{
    public string Id { get; set; }
    public int TotalPlan { get; set; }
    public DateTime? CreateAt { get; set; }
}

public class StatisticsPublicPlanDto
{
    public int? TotalPublicPlan { get; set; }
    public int? TotalCreatedPlan { get; set; }
    public int? TotalBean { get; set; }
    public int? TotalInterestRate { get; set; }
}

public class StatisticsPublicPlanResultDto
{
    public StatisticsPublicPlanDto StatisticsPublicPlan { get; set; }
    public StatisticsPublicPlanStatusDto[] StatisticsPublicPlanStatus { get; set; }
}
