#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace desu.life.Data.Models;

public class DesulifeIdentityRole : IdentityRole<int>
{
    [MaxLength(1024)] public string Description { get; set; }
}