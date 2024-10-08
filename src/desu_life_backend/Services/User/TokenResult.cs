﻿namespace desu.life.Services.User;

public class TokenResult
{
    public bool Success => Errors == null || !Errors.Any();
    public IEnumerable<string>? Errors { get; set; }

    public string? AccessToken { get; set; }

    public string? TokenType { get; set; }

    public int ExpiresIn { get; set; }

    public string? RefreshToken { get; set; }
}