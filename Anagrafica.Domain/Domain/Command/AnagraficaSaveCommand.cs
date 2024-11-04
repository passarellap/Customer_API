using ServiceUtilities.Command;
using System;
using TreePlat.Domain.Enumerations;

namespace AnagraficaAPI.Domain.Command
{
    public class AnagraficaSaveCommand : BaseUpdateCommand
    {
        public int? IdCollaboratore { get; set; }
        public int? IdIndirizzo { get; set; }
        public EStatoCivile StatoCivile { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string RagioneSociale { get; set; }
        public string Telefono { get; set; }
        public string Telefono2 { get; set; }
        public string Cellulare { get; set; }
        public string Fax { get; set; }
        public string EMail { get; set; }
        public string EMail2 { get; set; }
        public string SitoInternet { get; set; }
        public string SkypeId { get; set; }
        public string AltriRecapiti { get; set; }
        public int TipoCanalePreferito { get; set; }
        public string Note { get; set; }
        public int TipoConsenso { get; set; }
        public int? IdEditore { get; set; }
        public ETipoCliente TipoCliente { get; set; }
        public ETipoImpiego TipoImpiego { get; set; }
        public ETipoSesso TipoSesso { get; set; }
        public ETipoTitolo TipoTitolo { get; set; }
        public int? IdSettore { get; set; }
        public string AutoritaLuogoRilascio { get; set; }
        public string CodiceCRM { get; set; }
        public string CodiceFiscale { get; set; }
        public int CodicePadre { get; set; }
        public string CodicePadreCrm { get; set; }
        public DateTime DataRilascioDocumento { get; set; }
        public DateTime DataScadenzaDocumento { get; set; }
        public string NumeroDocumentoPresentato { get; set; }
        public string Piva { get; set; }
        public long TagCliente { get; set; }
        public ETipoDocumentoPresentato TipoDocumentoPresentato { get; set; }
        public string Idorigine { get; set; }
        public string NascitaLuogo { get; set; }
        public string NascitaProvincia { get; set; }
        public DateTime? NascitaData { get; set; }
        public bool EsenteAui { get; set; }
    }
}
