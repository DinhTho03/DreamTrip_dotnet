namespace brandportal_dotnet.Contracts.Plan.PlanManagement;

public class PageManagementPlanDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Departure { get; set; }
    public string Destination { get; set; }
    public bool? IsPublic { get; set; }
    public DateTime? CreatedAt { get; set; }
}