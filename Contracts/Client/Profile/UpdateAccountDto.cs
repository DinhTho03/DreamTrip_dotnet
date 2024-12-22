namespace brandportal_dotnet.Contracts.Client.Profile;

public class UpdateAccountDto
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Avatar { get; set; }
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmPassword { get; set; }
    
}