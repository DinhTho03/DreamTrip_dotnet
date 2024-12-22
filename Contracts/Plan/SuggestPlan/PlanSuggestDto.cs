namespace brandportal_dotnet.Contracts.Plan.SuggestPlan;

public record SuggestPlanDto
{
    public string Id { get; set; }
    public string? Type { get; set; }
    public string? Name { get; set; }
    public sbyte? Order { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public record ActivityPlanDto
{
    public string Id { get; set; }

    public string? Name { get; set; }

    public string? SuggestPlanId { get; set; }

    public DateTime? CreateAt { get; set; }

    public bool? IsActive { get; set; }
}