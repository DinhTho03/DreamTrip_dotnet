namespace brandportal_dotnet.Contracts.Client.Plan;

public class PlaceTimeLineDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Photos { get; set; }
    public string TimeActive { get; set; }
    public bool HasExperienced { get; set; }
}

public class PlaceTimeLineResponse
{
    public PlaceTimeLineDto[][] PlaceList { get; set; }
    public string[] DateInPlan { get; set; }
}