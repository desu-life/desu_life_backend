#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace desu.life.Data.Models;

public class UserLink
{
    [StringLength(16)]
    public string Osu { get; set; }

    [StringLength(32)]
    public string Discord { get; set; }

    [StringLength(16)]
    public string OneBot { get; set; }

    [StringLength(64)]
    public string OpenQQ { get; set; }

    [Key]
    [Required]
    public int UserId { get; set; }

    [Required]
    [ForeignKey(nameof(UserId))]
    public DesuLifeIdentityUser User { get; set; }
}