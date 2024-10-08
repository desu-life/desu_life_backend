using desu.life.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace desu.life;

public static class WebApplicationAuthorizationExtensions
{
    public static List<string> Policies { get; private set; } = new List<string>();

    /// <summary>
    ///     初始化默认的Authorization
    /// </summary>
    /// <remarks>
    ///     Role为用户组，Policy为具体的功能。<br />
    ///     一般来说，Policy是否合法首先验证Role，其次验证对应User的Policy开关。<br />
    ///     示例中使用Claim实现User具体Policy开关，或者也可使用其他方式。
    /// </remarks>
    /// <param name="services"></param>
    /// <example>
    ///     示例Roles: NormalUser, CommunityAdmin, ServerAdmin, etc.<br />
    ///     示例Policies: ManageUsers, ManageServers, ManageUserRoles, etc.
    ///     <code>
    ///         options.AddPolicy("RequireManageUsersRole", policy => policy
    ///             .RequireRole("ServerAdmin")
    ///             .RequireAssertion(k => !k.User.HasClaim("RequireManageUsersRole")
    ///                 // 基于Claim排除对应Policy，若Claim中存在对应Policy则该功能不可用
    ///                 // 同时需要api进行管理Claim: _userManager.AddClaimsAsync()
    ///         );
    ///     </code>
    /// </example>
    public static void AddDefaultAuthorization(this IServiceCollection services)
    {
        // 添加授权服务
        services.AddAuthorization(options =>
        {
            // example
            // 验证用户是否具有特定角色且所需的Policy没有被禁用
            //  options.AddPolicy("RequireManageUsersRole", policy => policy
            //    .RequireRole("ServerAdmin")
            //    .RequireAssertion(context =>
            //          !context.User.HasClaim(c => c.Type == "DisablePolicy" && c.Value == "RequireManageUsersRole")));

            // 允许带有特定声明的用户管理服务器
            // options.AddPolicy("ManageServers", policy => policy
            //    .RequireAssertion(context =>
            //        context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "ManageServers")));


            // 应用的示例
            // [Authorize(Policy = "CanManageEvents")]
            // public IActionResult ManageEvent()
            // {
            //     return View();
            // }
            //
            // [Authorize(Policy = "ReadAccess")]
            // public IActionResult PublicPage()
            // {
            //     return View();
            // }

            // Claim管理 UserService.cs -> AddUserClaimAsync/RemoveClaimAsync/GetUserClaimsAsync/UpdateUserClaimAsync

            // 基础 Basic
            options.AddPolicy("Login", policy => policy
                .RequireRole("Basic"));

            options.AddPolicy("ViewProfile", policy => policy
                .RequireRole("Basic"));

            options.AddPolicy("RefreshToken", policy => policy
               .RequireRole("Basic"));

            // 用户 User
            options.AddPolicy("EditProfile", policy => policy
                .RequireRole("User")
                .RequireAssertion(context => !context.User.HasClaim(c => c.Type == "DisablePolicy" && c.Value == "EditProfile")));

            options.AddPolicy("LinkAccount", policy => policy
                .RequireRole("User"));

            options.AddPolicy("ChangePassword", policy => policy
                .RequireRole("User"));

            options.AddPolicy("Customize", policy => policy
                .RequireRole("User")
                .RequireAssertion(context => !context.User.HasClaim(c => c.Type == "DisablePolicy" && c.Value == "Customize")));

            // 管理员 Administrator
            options.AddPolicy("ManageUsers", policy => policy
                .RequireRole("Administrator"));

            options.AddPolicy("ManageRoles", policy => policy
                .RequireRole("Administrator"));

            options.AddPolicy("Maintenance", policy => policy
                .RequireRole("Administrator"));

            options.AddPolicy("ManageServerSettings", policy => policy
                .RequireRole("Administrator"));

            options.AddPolicy("ManageAPITokens", policy => policy
                .RequireRole("Administrator"));

        });
    }

    public static async Task UseDefaultPoliciesAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        // 创建角色组
        var serviceProvider = scope.ServiceProvider;

        // 获取角色管理器
        var roleManager = serviceProvider.GetRequiredService<RoleManager<DesulifeIdentityRole>>();

        // 定义角色组
        string[] roles = ["System", "Bot", "Administrator", "User"];

        // 创建角色
        foreach (var role in roles)
        {
            var roleExist = await roleManager.RoleExistsAsync(role);
            // 如果角色不存在则创建
            if (!roleExist)
            {
                await roleManager.CreateAsync(new DesulifeIdentityRole { Name = role, Description = "" });
            }
        }

        // 更新描述
        await UpdateRoleDescription(roleManager);
    }

    private static async Task UpdateRoleDescription(RoleManager<DesulifeIdentityRole> roleManager)
    {
        // 更新角色描述
        foreach (var role in roleManager.Roles.ToList())
        {
            // 获取角色
            var newRole = await roleManager.FindByIdAsync(role.Id.ToString());

            // 如果角色不存在则跳过
            if (newRole == null) continue;

            // 更新角色描述
            role.Description = role.Name switch
            {
                "System" => "该角色具有系统权限",
                "Bot" => "该角色具有应答机器人权限",
                "Administrator" => "该角色具有管理员权限",
                "User" => "该角色具有用户权限",
                _ => role.Description
            };

            await roleManager.UpdateAsync(role);
        }
    }
}