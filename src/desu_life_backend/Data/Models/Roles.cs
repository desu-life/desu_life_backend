using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace desu.life.Data.Models;

public class DesulifeIdentityRole : IdentityRole<int>
{
    public string Description { get; set; }
}

// TODO: [FrZ] 修改角色组
public static class Roles
{
    public static void ConfigureAuthorization(this IServiceCollection services)
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

    public static async Task CreateRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<RoleManager<DesulifeIdentityRole>>();
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
        var groupRoleExist = await roleManager.RoleExistsAsync(groupName);
        if (!groupRoleExist)
        {
            await roleManager.CreateAsync(new DesulifeIdentityRole { Name = groupName, Description = "" });
        }

        foreach (var role in roles)
        {
            var groupRole = await roleManager.FindByNameAsync(groupName);
            if (groupRole == null) continue;
            await roleManager.AddClaimAsync(groupRole!, new Claim(ClaimTypes.Role, role));
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