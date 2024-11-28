namespace brandportal_dotnet.Contracts.Client.Plan;

public class PlanRequestDto
{
    public PlaceListDto[][] Data { get; set; }
    public string NamePlan { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Departure { get; set; }
    public string Destination { get; set; }
}

public record PlaceListDto
{
    public string PlaceId { get; set; }
    public Geometry Geometry { get; set; }
    public string Name { get; set; }
    public string Photos { get; set; }
    public double Rating { get; set; }
    public string Distance { get; set; }
    public string? Duration { get; set; }
    public int User_ratings_total { get; set; }
    public string? Url { get; set; }
    public string? International_phone_number { get; set; }
    public string? TimeOpen { get; set; }
    public string? TimeClose { get; set; }
    public string? Formatted_address { get; set; }
    public string? TitleNote { get; set; }
    public string? DescribeNote { get; set; }
}

public class UpdatePlanRequestDto
{
    public UpdatePlaceListDto[][] Data { get; set; }
}

public record UpdatePlaceListDto
{
    public string PlaceId { get; set; }
    public Geometry Geometry { get; set; }
    public string Name { get; set; }
    public string Photos { get; set; }
    public double Rating { get; set; }
    public string Distance { get; set; }
    public string? Duration { get; set; }
    public int User_ratings_total { get; set; }
    public string? Url { get; set; }
    public string? International_phone_number { get; set; }
    public string? TimeOpen { get; set; }
   
    public string? TimeClose { get; set; }
    public string? TimeActive { get; set; }
    public string? Formatted_address { get; set; }
    public string? TitleNote { get; set; }
    public string? DescribeNote { get; set; }
    public string? Price { get; set; }
    public bool? HasExperienced { get; set; }    
    public int? NumberDay { get; set; }
    
}

public record Geometry
{
    public Location Location { get; set; }
}

public record Location
{
    public double Lat { get; set; }
    public double Lng { get; set; }
}

public record PlanResponse
{
    public string Id { get; set; }
}