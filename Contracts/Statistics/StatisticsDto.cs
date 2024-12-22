namespace brandportal_dotnet.Contracts.Statistics;

public record StatisticsDto
{
    public int? GroupPlanCount { get; set; }
    public int? PlaceInPlanCount { get; set; }
    public int? HasExperiencedCount { get; set; }
    public int? HasExperienceCount { get; set; }    
}