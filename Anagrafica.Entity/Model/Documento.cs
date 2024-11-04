using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TreePlat.Domain.Enumerations;

namespace AnagraficaAPI.Entity.Model
{
    [Table("GES_tbl_Documento")]
    public partial class Documento
    {
        [Key]
        [Required]
        public int IdDocumento { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public ETipoEntitaDocumento TipoEntita { get; set; }

        [Required]
        public string Descrizione { get; set; }

        [Required]
        public string NomeFile { get; set; }

        [Required]
        public string Estensione { get; set; }

        [Required]
        public int Dimensione { get; set; }

        public string Url { get; set; }

        public DateTime? Created { get; set; }

        public string NomeFileUtente { get; set; }

        public virtual DocumentoCliente DocumentoCliente { get; set; }
    }
}
