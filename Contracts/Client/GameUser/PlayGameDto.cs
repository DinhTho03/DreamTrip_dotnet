namespace brandportal_dotnet.Contracts.Client.GameUser;

public class PlayGameDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Image { get; set; }
    public string? GameId { get; set; }
    
}