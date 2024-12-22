namespace brandportal_dotnet.Contracts.Plan.PlanExperience;

public class PagePlanExperienceDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string UserNameInherit { get; set; }
    public string Departure { get; set; }
    public string Destination { get; set; }
    public DateTime? CreatedAt { get; set; }
}