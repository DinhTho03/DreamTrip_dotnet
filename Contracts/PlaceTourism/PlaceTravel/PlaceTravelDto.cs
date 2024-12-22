namespace brandportal_dotnet.Contracts.PlaceTourism.PlaceTravel;

public record PlaceTravelDto
{
    public string Id { get; set; }
    public string? PlaceTourismGroupId { get; set; }
    public string? PlaceTourismGroupName { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public string Description { get; set; }
    public double? Rating { get; set; }
    public string OpeningTime { get; set; }
    public string ClosingTime { get; set; }
    public double? MinEntryFee { get; set; }
    public double? MaxEntryFee { get; set; }
    public DateTime? CreatedAt { get; set; }
    
    public bool IsActive { get; set; }
}