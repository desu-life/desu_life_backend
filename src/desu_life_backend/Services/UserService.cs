﻿using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using desu.life.Data;
using desu.life.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace desu.life.Services;

//https://www.c-sharpcorner.com/article/securing-asp-net-core-web-api-with-jwt-authentication-and-role-based-authorizati/
//https://www.cnblogs.com/xhznl/p/15406283.html
public class UserService : IUserService
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<DesuLifeIdentityUser> _userManager;

    public UserService(ApplicationDbContext applicationDbContext, JwtSettings jwtSettings,
        UserManager<DesuLifeIdentityUser> userManager)
    {
        _applicationDbContext = applicationDbContext;
        _jwtSettings = jwtSettings;
        _userManager = userManager;
    }

    public async Task<TokenResult> RegisterAsync(string username, string password, string email) // Todo: 参数增加用户组
    {
        var existingUser = await _userManager.FindByNameAsync(username);
        if (existingUser != null)
        {
            return new TokenResult
            {
                Errors = new[] { "user already exists!" }, //用户已存在
            };
        }

        var newUser = new DesuLifeIdentityUser { UserName = username, Email = email };
        var isCreated = await _userManager.CreateAsync(newUser, password);
        if (!isCreated.Succeeded)
        {
            return new TokenResult
            {
                Errors = isCreated.Errors.Select(p => p.Description)
            };
        }

        // TODO: 赋予用户组，_userManager.AddToRoleAsync()
        var roles = await _userManager.GetRolesAsync(newUser);
        return await GenerateJwtTokenAsync(newUser, roles);
    }

    public async Task<TokenResult> LoginAsync(string username, string password)
    {
        var existingUser = await _userManager.FindByNameAsync(username);
        if (existingUser == null)
        {
            return new TokenResult
            {
                Errors = new[] { "user does not exist!" }, //用户不存在
            };
        }

        var isCorrect = await _userManager.CheckPasswordAsync(existingUser, password);
        if (!isCorrect)
        {
            return new TokenResult
            {
                Errors = new[] { "wrong user name or password!" }, //用户名或密码错误
            };
        }

        var roles = await _userManager.GetRolesAsync(existingUser);
        return await GenerateJwtTokenAsync(existingUser, roles);
    }

    public async Task<TokenResult> RefreshTokenAsync(string token, string refreshToken)
    {
        var claimsPrincipal = GetClaimsPrincipalByToken(token);
        if (claimsPrincipal == null)
        {
            // 无效的token...
            return new TokenResult()
            {
                Errors = new[] { "1: Invalid request!" },
            };
        }

        var expiryDateUnix =
            long.Parse(claimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expiryDateTimeUtc = UnixTimeStampToDateTime(expiryDateUnix);
        if (expiryDateTimeUtc > DateTime.UtcNow)
        {
            // token未过期...
            return new TokenResult()
            {
                Errors = new[] { "2: Invalid request!" },
            };
        }

        var jti = claimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        var storedRefreshToken =
            await _applicationDbContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);
        if (storedRefreshToken == null)
        {
            // 无效的refresh_token...
            return new TokenResult()
            {
                Errors = new[] { "3: Invalid request!" },
            };
        }

        if (storedRefreshToken.ExpiryTime < DateTime.UtcNow)
        {
            // refresh_token已过期...
            return new TokenResult()
            {
                Errors = new[] { "4: Invalid request!" },
            };
        }

        if (storedRefreshToken.Invalidated)
        {
            // refresh_token已失效...
            return new TokenResult()
            {
                Errors = new[] { "5: Invalid request!" },
            };
        }

        if (storedRefreshToken.Used)
        {
            // refresh_token已使用...
            return new TokenResult()
            {
                Errors = new[] { "6: Invalid request!" },
            };
        }

        if (storedRefreshToken.JwtId != jti)
        {
            // refresh_token与此token不匹配...
            return new TokenResult()
            {
                Errors = new[] { "7: Invalid request!" },
            };
        }

        storedRefreshToken.Used = true;
        //_userDbContext.RefreshTokens.Update(storedRefreshToken);
        await _applicationDbContext.SaveChangesAsync();

        var dbUser = await _userManager.FindByIdAsync(storedRefreshToken.UserId.ToString());

        Debug.Assert(dbUser != null);

        var roles = await _userManager.GetRolesAsync(dbUser);
        return await GenerateJwtTokenAsync(dbUser, roles);
    }

    private ClaimsPrincipal? GetClaimsPrincipalByToken(string token)
    {
        Debug.Assert(_jwtSettings.SecurityKey != null);

        try
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecurityKey)),
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false // 解析token，注意这里的tokenValidationParameters，
                                         // 这个参数和Startup中设置的tokenValidationParameters唯一的区别是ValidateLifetime = false，
                                         // 不验证过期时间
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var claimsPrincipal =
                jwtTokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

            var validatedSecurityAlgorithm = validatedToken is JwtSecurityToken jwtSecurityToken
                                             && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                 StringComparison.InvariantCultureIgnoreCase);

            return validatedSecurityAlgorithm ? claimsPrincipal : null;
        }
        catch
        {
            return null;
        }
    }

    private async Task<TokenResult> GenerateJwtTokenAsync(DesuLifeIdentityUser user, IEnumerable<string> roles)
    {
        Debug.Assert(_jwtSettings.SecurityKey != null);
        Debug.Assert(user.UserName != null);

        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecurityKey);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var securityToken = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.UtcNow.Add(_jwtSettings.ExpiresIn),
            signingCredentials: credentials,
            notBefore: DateTime.UtcNow
        );
        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        //var tokenDescriptor = new SecurityTokenDescriptor
        //{
        //    Subject = new ClaimsIdentity(new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
        //        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        //    }),
        //    IssuedAt = DateTime.UtcNow,
        //    NotBefore = DateTime.UtcNow,
        //    Expires = DateTime.UtcNow.Add(_jwtSettings.ExpiresIn),
        //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
        //        SecurityAlgorithms.HmacSha256Signature)
        //};

        //var jwtTokenHandler = new JwtSecurityTokenHandler();
        //var securityToken = jwtTokenHandler.CreateToken(tokenDescriptor);
        //var token = jwtTokenHandler.WriteToken(securityToken);

        var refreshToken = new RefreshToken
        {
            JwtId = securityToken.Id,
            UserId = user.Id,
            CreationTime = DateTime.UtcNow,
            ExpiryTime = DateTime.UtcNow.AddMonths(6),
            Token = GenerateRandomNumber()
        };

        await _applicationDbContext.RefreshTokens.AddAsync(refreshToken);
        await _applicationDbContext.SaveChangesAsync();

        return new TokenResult
        {
            AccessToken = token,
            TokenType = "Bearer",
            RefreshToken = refreshToken.Token,
            ExpiresIn = (int)_jwtSettings.ExpiresIn.TotalSeconds,
        };
    }

    private static string GenerateRandomNumber(int len = 32)
    {
        var randomNumber = new byte[len];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        return DateTime.UnixEpoch.AddSeconds(unixTimeStamp).ToLocalTime();
    }
}