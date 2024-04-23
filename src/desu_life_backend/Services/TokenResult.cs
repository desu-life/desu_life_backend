﻿namespace desu_life_backend.Services;

public class TokenResult
{
    public bool Success => Errors == null || !Errors.Any();
    public IEnumerable<string> Errors { get; set; }
    
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
}