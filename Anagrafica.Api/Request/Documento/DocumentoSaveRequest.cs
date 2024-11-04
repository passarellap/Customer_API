namespace AnagraficaAPI.Request
{
    public class DocumentoSaveRequest
    {
        public int IdCliente { get; set; }
        public string Nomefile { get; set; }
        public string Estensione { get; set; }
        public string Descrizione { get; set; }
        public string Url { get; set; }
        public string NomeFileUtente { get; set; }
        public int Dimensione { get; set; }
    }
}
