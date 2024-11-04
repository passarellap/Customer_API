using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnagraficaAPI.Entity.Model
{
    [Table("GES_isa_DocumentoCliente")]
    public class DocumentoCliente
    {
        [Key]
        [Required]
        [Column("IDDocumento")]
        public int IdDocumento { get; set; }

        [Required]
        [Column("IDCliente")]
        public int IdCliente { get; set; }

        public virtual Documento Documento { get; set; }
        public virtual Cliente Cliente { get; set; }
    }
}
