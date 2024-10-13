namespace brandportal_dotnet.Contracts.Games;

public record PageGameDto
{
    public string Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? CateId { get; set; }
    public string? CateName { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? CreatedAt { get; set; }
}
