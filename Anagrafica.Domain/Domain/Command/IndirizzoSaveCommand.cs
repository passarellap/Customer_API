using ServiceUtilities.Command;

namespace AnagraficaAPI.Domain.Command
{
    public class IndirizzoSaveCommand : BaseUpdateCommand
    {
        public int? IdCitta { get; set; }

        public int IdComune { get; set; }

        public string Descrizione { get; set; }

        public string Civico { get; set; }

        public decimal Latitudine { get; set; }

        public decimal Longitudine { get; set; }

        public string Cap { get; set; }
    }
}
