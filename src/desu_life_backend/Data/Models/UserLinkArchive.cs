using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace desu.life.Data.Models
{

    public record UserLinkArchive
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public long ArchiveTime { get; set; }

        [Required]
        [Comment("绑定的平台")]
        [StringLength(16)]
        public string Platform { get; set; }

        [Comment("触发换绑的用户ID")]
        [Required] public int UserId { get; set; }

        [Required]
        [Comment("更换前的绑定信息")]
        [StringLength(64)]
        public string LinkInfoArchive { get; set; }

    }
}
