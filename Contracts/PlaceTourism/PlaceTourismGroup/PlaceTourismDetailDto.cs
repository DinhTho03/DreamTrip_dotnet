namespace brandportal_dotnet.Contracts.PlaceTourism.PlaceTourismGroup;

public record PlaceTourismDetailDto
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public bool IsActive { get; set; }
    public string PlaceTourismCateId { get; set; }
    
}