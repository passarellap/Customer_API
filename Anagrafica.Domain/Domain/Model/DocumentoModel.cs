namespace AnagraficaAPI.Domain.Model
{
    public class DocumentoModel
    {
        public int IdDocumento { get; set; }
        public int IdCliente { get; set; }
        public string Descrizione { get; set; }
        public string NomeFile { get; set; }
        public string Estensione { get; set; }
        public string Url { get; set; }
        public string NomeFileUtente { get; set; }
        public int Dimensione { get; set; }
    }
}
