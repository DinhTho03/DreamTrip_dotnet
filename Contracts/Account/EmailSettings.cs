﻿namespace brandportal_dotnet.Contracts.Account;

public class EmailSettings
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public string DisplayName { get; set; }
    public int Port { get; set; }
}