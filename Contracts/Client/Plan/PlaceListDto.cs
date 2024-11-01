namespace brandportal_dotnet.Contracts.Client.Plan;

public class PlanRequestDto
{
    public PlaceListDto[][] Data { get; set; }
    public string NamePlan { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
public record PlaceListDto
{
    public string PlaceId { get; set; }
    public Geometry Geometry { get; set; }
    public string Name { get; set; }
    public string Photos { get; set; }
    public double Rating { get; set; }
    public string Distance { get; set; }
    public int User_ratings_total { get; set; }
}

public record Geometry
{
    public Location Location { get; set; }
}

public record Location
{
    public double  Lat { get; set; }
    public double  Lng { get; set; }
}

public record PlanResponse
{
    public string Id { get; set; }
}