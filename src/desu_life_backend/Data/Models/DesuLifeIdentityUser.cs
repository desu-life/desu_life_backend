using Microsoft.AspNetCore.Identity;

namespace desu.life.Data.Models;

public class DesuLifeIdentityUser : IdentityUser<int>
{
    public long RegisterTime { get; set; }
}