using System.Security.Claims;
using desu.life.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace desu.life;

public static class WebApplicationAuthorizationExtensions
{
    // TODO: [FrZ] 修改角色组

    // Roles: NormalUser, CommunityAdmin, ServerAdmin, etc.
    // Policies: ManageUsers, ManageServers, ManageUserRoles, etc.
    // options.AddPolicy("RequireManageUsersRole", policy => policy.RequireRole("ServerAdmin").RequireClaim("target_claim")); //基于claim验证对应功能
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