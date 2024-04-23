namespace desu.life.Services;

public interface IUserService
{ 
    Task<TokenResult> RegisterAsync(string username, string password, string email);
    
    Task<TokenResult> LoginAsync(string username, string password);

    Task<TokenResult> RefreshTokenAsync(string token, string refreshToken);
}