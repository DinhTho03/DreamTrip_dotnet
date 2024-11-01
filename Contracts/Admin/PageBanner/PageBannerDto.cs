namespace brandportal_dotnet.Contracts.PageBanner;

public record PageBannerDto
{
    public string Id { get; set; }
    public dynamic? Order { get; set; }
    public string? Image { get; set; }
    public string? Title { get; set; }
    public string? Action { get; set; }
    public string? ActionParams { get; set; }
    public DateTime? UpdatedAt { get; set; }
}