namespace brandportal_dotnet.Contracts.Client.Plan;

public record PlanDetailDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Photos { get; set; }
    public string TimeActive { get; set; }
    public double? Rating { get; set; }
    public bool HasExperienced { get; set; }
    public string? Distance { get; set; }
    public string? Duration { get; set; }
    public string? Url { get; set; }
    public string? International_phone_number { get; set; }
    public string? TimeOpen { get; set; }
    public string? TimeClose { get; set; }
    public string? Formatted_address { get; set; }
    public string? TitleNote { get; set; }
    public string? DescribeNote { get; set; }
    public string? Price { get; set; }
    public int? UserRatingsTotal { get; set; }
    public string? PlaceId { get; set; }
}

public record PlanDetailDtoResponse
{
    public PlanDetailDto[][] PlanDetail { get; set; }
    public string[] DateInPlan { get; set; }
    public string? Departure { get; set; }
    public string? Destination { get; set; }

}