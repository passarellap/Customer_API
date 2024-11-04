using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnagraficaAPI.Entity.Model
{
    [Table("GEO_tbl_Indirizzo")]
    public partial class Indirizzo : BaseEntity
    {
        public Indirizzo()
        {
        }

        [Required]
        [Column("IDComune")]
        public int IdComune { get; set; }

        [Column("IDCitta")]
        public int? IdCitta { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Descrizione { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string Civico { get; set; }

        [Required]
        [Column(TypeName = "numeric(18, 15)")]
        public decimal Latitudine { get; set; }

        [Required]
        [Column(TypeName = "numeric(18, 15)")]
        public decimal Longitudine { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Cap { get; set; }

        public Cliente Cliente { get; set; }
    }
}
