namespace desu.life.Settings;

public class SmtpSettings
{
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string Secure { get; init; }
    public required string Sender { get; init; }
}