namespace brandportal_dotnet.Contracts.PageBanner;

public class PageBannerCreateDto
{
    public string? Tagline { get; set; }
    public string? ActionName { get; set; }
    public string Image { get; set; } = null!;
    public string? Title { get; set; }
    public string? EndPoint { get; set; }
    public string EndPointId { get; set; }
    public string EndPointName { get; set; }
    public string? Url { get; set; }
    public int? AllowAllOutlet { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartEffectiveDate { get; set; }
    public DateTime? EndEffectiveDate { get; set; }
}

