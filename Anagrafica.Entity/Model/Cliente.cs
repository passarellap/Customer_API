using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using TreePlat.Domain.Enumerations;

namespace AnagraficaAPI.Entity.Model
{

    [Table("CLI_tbl_Cliente")]
    public partial class Cliente : AgenziaEntityBase
    {
        public Cliente()
        {
            RelClientiTags = new HashSet<RelClientiTags>();
            DocumentoCliente = new HashSet<DocumentoCliente>();
        }

        [Column("IDCollaboratore")]
        public int? IdCollaboratore { get; set; }

        [Column("IDIndirizzo")]
        public int? IdIndirizzo { get; set; }

        [Required]
        public EStatoCliente Stato { get; set; }

        [Required]
        public EStatoCivile StatoCivile { get; set; }

        [Column(TypeName = "int")]
        public ETipoCliente TipoCliente { get; set; }

        [Column(TypeName = "int")]
        public ETipoImpiego TipoImpiego { get; set; }

        [Column(TypeName = "int")]
        public ETipoSesso TipoSesso { get; set; }

        [Column(TypeName = "int")]
        public ETipoTitolo TipoTitolo { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Nome { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Cognome { get; set; }

        [Required]
        [Column("RagSociale", TypeName = "nvarchar(100)")]
        public string RagioneSociale { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string Telefono { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string Telefono2 { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string Cellulare { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string Fax { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(320)")]
        public string EMail { get; set; }

        [Required]
        [Column("EMail2", TypeName = "nvarchar(320)")]
        public string Email2 { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string SitoInternet { get; set; }

        [Required]
        [Column("SkypeID", TypeName = "nvarchar(320)")]
        public string SkypeId { get; set; }

        [Required]
        [MaxLength(200)]
        public string AltriRecapiti { get; set; }

        public ETipoCanalePreferito TipoCanalePreferito { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Note { get; set; }

        [Required]
        public int TipoConsenso { get; set; }

        [Column(name: "IdEditore")]
        public int? IdEditore { get; set; }

        [Column(name: "IdSettore")]
        public int? IdSettore { get; set; }

        [Required]
        [Column(TypeName = "varchar(30)")]
        public string AutoritaLuogoRilascio { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string CodiceCRM { get; set; }

        [Required]
        [Column(TypeName = "varchar(16)")]
        public string CodiceFiscale { get; set; }

        [Required]
        public int CodicePadre { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string CodicePadreCrm { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime DataRilascioDocumento { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime DataScadenzaDocumento { get; set; }

        [Required]
        [Column(TypeName = "varchar(15)")]
        public string NumeroDocumentoPresentato { get; set; }

        [Required]
        [Column("PIva", TypeName = "varchar(11)")]
        public string Piva { get; set; }

        [Required]
        [Column(TypeName = "bigint")]
        public long TagCliente { get; set; }

        [Required]
        public ETipoDocumentoPresentato TipoDocumentoPresentato { get; set; }

        [Column(TypeName = "nvarchar(320)")]
        public string NascitaLuogo { get; set; }

        [Column(TypeName = "nvarchar(320)")]
        public string NascitaProvincia { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? NascitaData { get; set; }

        [Required]
        [Column(TypeName = "bit")]
        public bool EsenteAui { get; set; }

        public virtual Indirizzo Indirizzo { get; set; }

        public virtual ICollection<RelClientiTags> RelClientiTags { get; set; }
        public virtual ICollection<DocumentoCliente> DocumentoCliente { get; set; }
    }
}