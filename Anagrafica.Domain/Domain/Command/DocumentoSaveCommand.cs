using System;
using System.Collections.Generic;
using System.Text;

namespace AnagraficaAPI.Domain.Command
{
    public class DocumentoSaveCommand
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
