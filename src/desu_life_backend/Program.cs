using System.Text;
using desu.life.Data;
using desu.life.Data.Models;
using desu.life.Services;
using desu.life.Services.Email;
using desu.life.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace desu.life;

public class Program
{
    public static string connectionString;

    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);

        var app = builder.Build();

        await ConfigureAsync(app);
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                           throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, new MariaDbServerVersion(new Version(10, 11, 7)))
        );

        builder.Services
            .AddIdentityCore<DesuLifeIdentityUser>(config =>
            {
                config.SignIn.RequireConfirmedAccount = true;
                config.Tokens.ProviderMap.Add("CustomEmailConfirmation",
                    new TokenProviderDescriptor(typeof(EmailConfirmationTokenProvider<DesuLifeIdentityUser>)));
                config.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
            })
            .AddRoles<DesulifeIdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        //https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-8.0
        builder.Services.AddTransient<EmailConfirmationTokenProvider<DesuLifeIdentityUser>>();

        // Todo: create roles: https://stackoverflow.com/questions/42471866/how-to-create-roles-in-asp-net-core-and-assign-them-to-users
        builder.Services
            .AddScoped<IRoleStore<DesulifeIdentityRole>, RoleStore<DesulifeIdentityRole, ApplicationDbContext, int>>();
        builder.Services.AddScoped<RoleManager<DesulifeIdentityRole>>();
        builder.Services.AddDefaultAuthorization();

        // JWT
        var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>() ??
                          throw new InvalidOperationException($"Settings section '{nameof(JwtSettings)}' not found.");
        if (string.IsNullOrEmpty(jwtSettings.SecurityKey))
        {
            throw new InvalidOperationException($"SecurityKey of '{nameof(JwtSettings)}' not set.");
        }

        builder.Services.AddSingleton(jwtSettings);
        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecurityKey)),
                    ClockSkew = TimeSpan.Zero,
                };
            });

        // Services.EmailSender
        var emailSettings = builder.Configuration.GetSection(nameof(SmtpSettings)).Get<SmtpSettings>() ??
                            throw new InvalidOperationException(
                                $"Settings section '{nameof(SmtpSettings)}' not found.");
        if (string.IsNullOrEmpty(emailSettings.Host))
        {
            throw new InvalidOperationException($"Host of '{nameof(SmtpSettings)}' not set.");
        }

        builder.Services.AddSingleton(emailSettings);
        builder.Services.AddSingleton<IEmailSender, EmailSender>(provider =>
        {
            var smtpSettings = provider.GetRequiredService<SmtpSettings>();

            return new EmailSender(smtpSettings.Host, smtpSettings.Port, smtpSettings.Username,
                smtpSettings.Password, smtpSettings.Secure, smtpSettings.Sender);
        });
        // builder.Services.AddTransient<IEmailSender, EmailSender>(
        //     provider =>
        //     {
        //         var smtpSettings = provider.GetRequiredService<SmtpSettings>();
        //
        //         return new EmailSender(smtpSettings.Host, smtpSettings.Port, smtpSettings.Username,
        //             smtpSettings.Password, smtpSettings.Secure, smtpSettings.Sender);
        //     });

        // Add services to the container.
        // builder.Services.AddTransient<IEmailSender, EmailSender>();
        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    private static async Task ConfigureAsync(WebApplication app)
    {
        // 创建角色组
        await app.UseDefaultPoliciesAsync();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}