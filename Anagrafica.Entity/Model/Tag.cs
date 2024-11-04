using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnagraficaAPI.Entity.Model
{
    [Table("CLI_tbl_Tag")]
    public partial class Tag : AgenziaEntityBase
    {
        public Tag()
        {
            RelClientiTags = new HashSet<RelClientiTags>();
        }

        [Required]
        [Column("IDCollaboratore")]
        public int IdCollaboratore { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string TagName { get; set; }

        [Required]
        [Column(TypeName = "char(9)")]
        public string Color { get; set; }

        public virtual ICollection<RelClientiTags> RelClientiTags { get; set; }
    }
}