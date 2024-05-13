#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace desu.life.Data.Models;

//TODO milki 240506: remigrate test db
public class DesulifeIdentityRole : IdentityRole<int>
{
    [MaxLength(1024)] public string Description { get; set; }
}