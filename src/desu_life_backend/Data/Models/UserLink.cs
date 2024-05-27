#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace desu.life.Data.Models;

//TODO milki 240527: [StringLength(xx)] of strings
public class UserLink
{
    public string Osu { get; set; }

    public string Discord { get; set; }

    public string OneBot { get; set; }

    public string OpenQQ { get; set; }

    [Key]
    [Required]
    public int UserId { get; set; }

    [Required]
    [ForeignKey(nameof(UserId))]
    public DesuLifeIdentityUser User { get; set; }
}