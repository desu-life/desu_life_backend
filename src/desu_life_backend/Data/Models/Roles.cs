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
            options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Admin"));
            options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
            options.AddPolicy("RequireGuestRole", policy => policy.RequireRole("Guest"));
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
            if (!roleExist) await roleManager.CreateAsync(new DesulifeIdentityRole { Name = role });
        }

        // 更新描述
        await UpdateRoleDescription(roleManager);
    }

    private static async Task CreateGroupRole(RoleManager<DesulifeIdentityRole> roleManager, string groupName, string[] roles)
    {
        var groupRoleExist = await roleManager.RoleExistsAsync(groupName);
        if (!groupRoleExist)
        {
            await roleManager.CreateAsync(new DesulifeIdentityRole { Name = groupName });
        }

        foreach (var role in roles)
        {
            var groupRole = await roleManager.FindByNameAsync(groupName);
            await roleManager.AddClaimAsync(groupRole!, new Claim(ClaimTypes.Role, role));
        }
    }

    private static async Task UpdateRoleDescription(RoleManager<DesulifeIdentityRole> roleManager)
    {
        foreach (var role in roleManager.Roles)
        {
            switch (role.Name)
            {
                case "ManageUsers":
                    role.Description = "该角色具有用户管理权限。";
                    break;
                case "ManageServers":
                    role.Description = "该角色具有服务器管理权限。";
                    break;
                case "ManageUserRoles":
                    role.Description = "该角色具有用户角色管理权限。";
                    break;
                case "Customize":
                    role.Description = "该角色具有自定义权限。";
                    break;
                case "Login":
                    role.Description = "该角色具有登录权限。";
                    break;
            }
            await roleManager.UpdateAsync(role);
        }
    }

}