namespace brandportal_dotnet.Contracts.Account
{
    public class UserDto
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public string? Avatar { get; set; }
    }
}
