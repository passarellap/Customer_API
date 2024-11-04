using ControllerUtilities.Response;

namespace AnagraficaAPI.Response
{
    public class IndirizzoResponseDto : BaseResponseDto
    {
        public string Descrizione { get; set; }
        public int? IdCitta { get; set; }
        public int IdComune { get; set; }
        public string Civico { get; set; }
        public string Cap { get; set; }
        public decimal Latitudine { get; set; }
        public decimal Longitudine { get; set; }
    }
}
