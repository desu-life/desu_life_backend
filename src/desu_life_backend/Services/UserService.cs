using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using desu.life.Data;
using desu.life.Data.Models;
using desu.life.Responses;
using desu.life.Settings;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace desu.life.Services;

//https://www.c-sharpcorner.com/article/securing-asp-net-core-web-api-with-jwt-authentication-and-role-based-authorizati/
//https://www.cnblogs.com/xhznl/p/15406283.html
public class UserService : IUserService
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<DesuLifeIdentityUser> _userManager;
    private readonly IEmailSender _emailSender;

    public UserService(ApplicationDbContext applicationDbContext, JwtSettings jwtSettings,
        UserManager<DesuLifeIdentityUser> userManager, IEmailSender emailSender)
    {
        _applicationDbContext = applicationDbContext;
        _jwtSettings = jwtSettings;
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public async Task<TokenResult> RegisterAsync(string username, string password, string email)
    {
        var existingUser = await _userManager.FindByNameAsync(username);
        
        if (existingUser != null)  // 非已验证邮箱用户重新注册
        {

            if (!existingUser.EmailConfirmed)
            {                    
                var existingUserEmailSendResult = await SendEmailAsync(email);
                if (!existingUserEmailSendResult.Success)
                {
                    return new TokenResult
                    {
                        Errors = new[] { "user already exists but email not confirmed! and email send failed!" }
                            
                    };
                }

                return new TokenResult
                {
                    Errors = new[] { "user already exists but email not confirmed! a new confirmation email has been sent to your email address." },
                };
            }
            return new TokenResult
            {
                Errors = new[] { "user already exists!" },
            };
        }

        var newUser = new DesuLifeIdentityUser
        {
            UserName = username,
            Email = email,
            RegisterTime = DateTimeOffset.Now.ToUnixTimeSeconds()
        };
        
        var isCreated = await _userManager.CreateAsync(newUser, password);
        
        if (!isCreated.Succeeded)
        {
            return new TokenResult
            {
                Errors = isCreated.Errors.Select(p => p.Description)
            };
        }

        // 注册阶段不赋予基本角色，等待邮箱验证
        
        // 赋予基本角色
        // string[] baseRoles = ["Login", "Customize"]; // 基本权限
        // foreach (var role in baseRoles)
        // {
        //     var addToRoleResult = await _userManager.AddToRoleAsync(newUser, role);
        //     if (!addToRoleResult.Succeeded)
        //         return new TokenResult { Errors = addToRoleResult.Errors.Select(p => p.Description) };
        //
        // }

        // 赋予角色组
        // var addToRoleGroupResult = await _userManager.AddToRoleAsync(newUser, "UserGroup"); // User用户组
        // if (!addToRoleGroupResult.Succeeded)
        //     return new TokenResult { Errors = addToRoleGroupResult.Errors.Select(p => p.Description) };
        //
        //
        // var roles = await _userManager.GetRolesAsync(newUser);
        
        var emailSendResult = await SendEmailAsync(email);
        if (!emailSendResult.Success)
        {
            return new TokenResult
            {
                Errors = new[] { "email send failed!" }
            };
        }
        
        return await GenerateJwtTokenAsync(newUser, await _userManager.GetRolesAsync(newUser));  // TODO: 该行报错 System.NotSupportedException: Store does not implement IUserRoleStore<TUser>.
    }

    public async Task<TokenResult> SendEmailAsync(string email)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        
        if (existingUser == null)
        {
            return new TokenResult
            {
                Errors = new[] { "user does not exist!" },
            };
        }

        if (existingUser.EmailConfirmed)
        {
            return new TokenResult
            {
                Errors = new[] { "email already confirmed!" },
            };
        }
        
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(existingUser);
        // token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var callbackUrl = $"{ConfigurationManager.AppSettings.GetValues("Host.Api")}/api/EmailConfirm" +
                          $"?token={token}";
        await _emailSender.SendEmailAsync(email, "Confirm your email",
            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
            );
        
        return new TokenResult {};
    }

    public async Task<TokenResult> EmailConfirmAsync(string email, string token)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser == null)
        {
            return new TokenResult
            {
                Errors = new[] { "user does not exist!" },
            };
        }
        if (existingUser.EmailConfirmed)
        {
            return new TokenResult
            {
                Errors = new[] { "email already confirmed!" },
            };
        }

        var confirmResult = await _userManager.ConfirmEmailAsync(existingUser, token);
        if (!confirmResult.Succeeded)
        {
            return new TokenResult
            {
                Errors = confirmResult.Errors.Select(p => p.Description)
            };
        }
        
        // 赋予角色与角色组
        var addToRolesResult = await _userManager.AddToRolesAsync(existingUser, ["Login", "Customize"]);
        if (!addToRolesResult.Succeeded)
        {
            return new TokenResult { Errors = addToRolesResult.Errors.Select(p => p.Description) };
        }
        
        var addToRoleGroupResult = await _userManager.AddToRoleAsync(existingUser, "UserGroup");
        if (!addToRoleGroupResult.Succeeded)
            return new TokenResult { Errors = addToRoleGroupResult.Errors.Select(p => p.Description) };
        
        // TODO: 这里到底应不应该返回这个，然后让上一级来判断是否返回HTML，还是说这里直接返回HTML
        return await GenerateJwtTokenAsync(existingUser, await _userManager.GetRolesAsync(existingUser));
    }

    public async Task<TokenResult> LoginAsync(string username, string password)
    // TODO: 改为email登录
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
        
        if (!await _userManager.IsEmailConfirmedAsync(existingUser))
        {
            await SendEmailAsync(existingUser.Email!);
            return new TokenResult
            {
                Errors = new[] { "wrong user name or password!" }, //邮箱未验证
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