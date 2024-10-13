namespace brandportal_dotnet.Contracts.Admin.User;

public record ListUserDto
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool? IsDeleted { get; set; }
}