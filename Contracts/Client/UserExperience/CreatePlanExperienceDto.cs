namespace brandportal_dotnet.Contracts.UserExperience;

public class CreatePlanExperienceDto
{
    public string UserId { get; set; }  
    public string GroupId { get; set; }  
    public string UserExperienceId { get; set; }  
    public DateTime StartDate { get; set; }  
    public DateTime EndDate { get; set; }  
}