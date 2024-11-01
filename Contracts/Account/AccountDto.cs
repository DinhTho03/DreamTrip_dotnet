namespace brandportal_dotnet.Contracts.Account
{
    public class AccountDto
    {
        public string? Email { get; set; }
        public string? Otptext { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
