using ServiceUtilities.Querying;
using System;
using System.Collections.Generic;

namespace AnagraficaAPI.Domain.Command
{
    public class AnagraficaFilterCommand : QueryStringParameters
    {
        public int? Id{ get; set; }
        public int? Stato { get; set; }
        public IEnumerable<int> Ids { get; set; }
        public string Nominativo { get; set; }
        public string Telefono { get; set; }
        public string EMail { get; set; }
        public int? TipoCliente { get; set; }
        public DateTime? NascitaData { get; set; }
        public IEnumerable<int> IdsTag { get; set; }
        public string FreeText { get; set; }
        public string PIva { get; set; }
    }
}
