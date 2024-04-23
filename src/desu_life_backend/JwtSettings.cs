namespace desu_life_backend;

public class JwtSettings
{
    public string? SecurityKey { get; set; }
    public TimeSpan ExpiresIn { get; set; }
}