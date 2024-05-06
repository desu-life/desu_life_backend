using System.Security.Claims;
using desu.life.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace desu.life;

// TODO: [FrZ] 修改角色组
public static class WebApplicationAuthorizationExtensions
{
    /// <summary>
    /// 初始化默认的Authorization
    /// </summary>
    /// <remarks>
    /// Role为用户组，Policy为具体的功能。<br/>
    /// 一般来说，Policy是否合法首先验证Role，其次验证对应User的Policy开关。<br/>
    /// 示例中使用Claim实现User具体Policy开关，或者也可使用其他方式。
    /// </remarks>
    /// <param name="services"></param>
    /// <example>
    ///     示例Roles: NormalUser, CommunityAdmin, ServerAdmin, etc.<br/>
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
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireManageUsersRole", policy => policy.RequireRole("ManageUsers"));
            options.AddPolicy("RequireManageServersRole", policy => policy.RequireRole("ManageServers"));
            options.AddPolicy("RequireManageUserRolesRole", policy => policy.RequireRole("ManageUserRoles"));
            options.AddPolicy("RequireCustomizeRole", policy => policy.RequireRole("Customize"));
            options.AddPolicy("RequireLoginRole", policy => policy.RequireRole("Login"));
        });
    }

    public static async Task UseDefaultPoliciesAsync(this WebApplication app)
    {
        var serviceProvider = app.Services;

        var roleManager = serviceProvider.GetRequiredService<RoleManager<DesulifeIdentityRole>>();
        string[] roles =
            ["ManageUsers", "ManageServers", "ManageUserRoles",
            "Customize", "Login"];

        // 角色组
        await CreateGroupRole(roleManager, "AdminGroup", ["ManageUsers", "ManageServers", "ManageUserRoles"]);
        await CreateGroupRole(roleManager, "UserGroup", []);

        foreach (var role in roles)
        {
            var roleExist = await roleManager.RoleExistsAsync(role);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new DesulifeIdentityRole { Name = role, Description = "" });
            }
        }

        // 更新描述
        await UpdateRoleDescription(roleManager);
    }

    private static async Task CreateGroupRole(RoleManager<DesulifeIdentityRole> roleManager, string groupName, string[] roles)
    {
        var groupRole = await roleManager.FindByNameAsync(groupName);
        if (groupRole == null)
        {
            groupRole = new DesulifeIdentityRole { Name = groupName };
            await roleManager.CreateAsync(groupRole);
        }

        if (roles.Length == 0) return;

        var claims = (await roleManager.GetClaimsAsync(groupRole)).Select(k => k.ToString()).ToHashSet();
        foreach (var role in roles)
        {
            var claim = new Claim(ClaimTypes.Role, role);
            if (claims.Contains(claim.ToString())) continue;
            await roleManager.AddClaimAsync(groupRole, claim);
        }
    }

    private static async Task UpdateRoleDescription(RoleManager<DesulifeIdentityRole> roleManager)
    {
        foreach (var role in roleManager.Roles.ToList())
        {
            var newRole = await roleManager.FindByIdAsync(role.Id.ToString());
            if (newRole == null) continue;
            role.Description = role.Name switch
            {
                "ManageUsers" => "该角色具有用户管理权限。",
                "ManageServers" => "该角色具有服务器管理权限。",
                "ManageUserRoles" => "该角色具有用户角色管理权限。",
                "Customize" => "该角色具有自定义权限。",
                "Login" => "该角色具有登录权限。",
                _ => role.Description
            };

            await roleManager.UpdateAsync(role);
        }
    }

}