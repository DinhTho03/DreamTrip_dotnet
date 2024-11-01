namespace brandportal_dotnet.Contracts.PageBanner;

public record PageBannerDetailDto
{
    public string Id { get; set; }
    public string? Tagline { get; set; }
    public string? Image { get; set; }
    public string? Title { get; set; }
    public string? ActionName { get; set; }
    public string? ActionParams { get; set; }
    public string? EndPointId { get; set; }
    public string? EndPointName { get; set; }
    public string? EndPoint { get; set; }
    public int? AllowAllOutlet { get; set; }
    public int? Position { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartEffectiveDate { get; set; }
    public DateTime? EndEffectiveDate { get; set; }
}