#nullable disable

using Microsoft.AspNetCore.Identity;

namespace desu.life.Data.Models;

public class DesuLifeIdentityUser : IdentityUser<int>
{
    // TODO: 根据实际情况增加IdentityUser（用户模型类）的字段，符合业务需求
    
    public long RegisterTime { get; set; }

}