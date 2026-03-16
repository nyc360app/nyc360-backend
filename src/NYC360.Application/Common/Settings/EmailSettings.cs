namespace NYC360.Application.Common.Settings;

public class EmailSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public Dictionary<string, EmailAccount> Accounts { get; set; } = new();
}

public class EmailAccount
{
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromDisplayName { get; set; } = "NYC360";
}