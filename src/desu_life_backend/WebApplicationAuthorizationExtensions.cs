using desu.life.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace desu.life;

public static class WebApplicationAuthorizationExtensions
{
    /// <summary>
    ///     初始化默认的Authorization
    /// </summary>
    /// <remarks>
    ///     Role为角色，Policy为具体的功能，对应前端的按钮/菜单和后端的一个/几个接口。<br />
    ///     一般来说，Policy是否合法首先验证Role，其次验证对应User的Policy开关。<br />
    ///     目前仅做角色粒度的权限控制，如果后续有业务需要支持用户粒度的权限控制，可以按example中的联用方式，针对某个Policy启用基于Role和Claim的控制
    /// </remarks>
    /// <param name="services"></param>
    public static void AddDefaultAuthorization(this IServiceCollection services)
    {
        // 添加授权服务
        services.AddAuthorization(options =>
        {
            // example
            // 联用Role和Claim做权限控制
            //  options.AddPolicy("RequireManageUsersRole", policy => policy
            //    .RequireRole("ServerAdmin")
            //    .RequireAssertion(context =>
            //          !context.User.HasClaim(c => c.Type == "DisablePolicy" && c.Value == "RequireManageUsersRole")));

            // 纯用Claim做用户粒度的权限控制
            // options.AddPolicy("ManageServers", policy => policy
            //    .RequireAssertion(context =>
            //        context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "ManageServers")));


            // Controller层用Attribute做权限控制的示例
            // [Authorize(Policy = "CanManageEvents")]

            // [Authorize(Roles = "Administrator")]


            // AspNetCore.Identity提供的Claim管理API：
            // userManager.GetClaimsAsync/userManager.RemoveClaimAsync/userManager.AddClaimAsync


            // 用户 User
            options.AddPolicy("EditProfile", policy => policy
                .RequireRole("User", "Administrator"));

            options.AddPolicy("LinkAccount", policy => policy
                .RequireRole("User", "Administrator"));

            options.AddPolicy("ChangePassword", policy => policy
                .RequireRole("User", "Administrator"));

            // 功能开发完成后 在此继续添加权限


            // 管理员 Administrator
            options.AddPolicy("AddActivity", policy => policy
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