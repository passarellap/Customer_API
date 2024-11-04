using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnagraficaAPI.Entity.Model
{
    [Table("CLI_rel_ClienteTag")]
    public partial class RelClientiTags
    {
        [Key, Column("IDCliente", Order = 0), Required]
        public int IdCliente { get; set; }

        [Key, Column("IDTag", Order = 1), Required]
        public int IdTag { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Tag Tag { get; set; }
    }
}