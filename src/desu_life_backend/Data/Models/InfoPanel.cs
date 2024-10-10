using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace desu.life.Data.Models
{
    public class InfoPanel
    {
        [Key]
        [Required]
        [Comment("主键")]
        public int Id { get; set; }

        [Required]
        [Comment("Panel标题")]
        [StringLength(32)]
        public string Title { get; set; }

        [Required]
        [Comment("Panel HTML内容")]
        public string Content { get; set; }

        [Required]
        [Comment("是否审核通过")]
        public bool Audited { get; set; }

        [Required]
        [Comment("是否已应用")]
        public bool Applied { get; set; }

        [Required]
        [Comment("创建时间")]
        public DateTime CreationTime { get; set; }

        [Required]
        [Comment("修改时间")]
        public DateTime LastModifiedTime { get; set; }

        [Required]
        [Comment("创建者ID")]
        public int AuthorId { get; set; }

        [Required]
        [ForeignKey(nameof(AuthorId))]
        public DesuLifeIdentityUser Author { get; set;}

    }
}
