namespace brandportal_dotnet.Contracts.Client.Profile;

public record ProFileDto
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string? Avatar { get; set; }
    public int? Point { get; set; }
}