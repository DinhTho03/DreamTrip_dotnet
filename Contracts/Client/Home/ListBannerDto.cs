namespace brandportal_dotnet.Contracts.User.Home;

public class ListBannerDto
{
    public string Id { get; set; }
    public string EndpointId { get; set; }
    public string Image { get; set; }
    public int? PageOrder { get; set; }
    public string PageTitle { get; set; }
    public string ActionName { get; set; }
}