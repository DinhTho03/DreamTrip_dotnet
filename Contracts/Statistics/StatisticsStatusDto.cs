namespace brandportal_dotnet.Contracts.Statistics;

public record StatisticListDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Departure { get; set; }
    public string Destination { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? CreateAt { get; set; }
}

public record ForDateStatus
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public record StatisticStatusDto
{
    public int? GroupPlanCount { get; set; }
    public int? HasExperiencedCount { get; set; }
    public int? HasExperiencedPercent { get; set; }
    public int? HasExperienceCount { get; set; }
    public int? HasExperiencePercent { get; set; }
    public StatisticListDto[] StatisticList { get; set; }
}

