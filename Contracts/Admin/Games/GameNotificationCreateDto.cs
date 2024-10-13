namespace brandportal_dotnet.Contracts.Games;

public class GameNotificationCreateDto
{
    public string? Id { get; set; }
    public string? NotificationType { get; set; }
    public int? SubId { get; set; }
    public string? ObjectId { get; set; }
    public string? ObjectSubId { get; set; }
    public string? Title { get; set; }
    public string? TitleShow { get; set; }
    public string? Description { get; set; }
    public string? DescriptionShow { get; set; }
    public string? Background { get; set; }
    public string? ParamsShow { get; set; }
}