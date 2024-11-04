using AnagraficaAPI.Entity.Model;
using System.Collections.Generic;
using TreePlat.Domain.Enumerations;

namespace UnitTest.AnagraficaAPI
{
    public static class MockDatabase
    {
        public static List<Cliente> GetClientiList()
        {
            List<Cliente> clienti = new()
            {
                new()
                {
                    Id = 1,
                    TipoCliente = ETipoCliente.PersonaFisica,
                    Nome = "giancarlo",
                    Cognome = "verdi",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = "",
                    Telefono = "1333 12224",
                    Telefono2 = "",
                    Cellulare = "",
                    EMail = "abcdefg@gmail.com",
                    Email2 = ""
                },
                new()
                {
                    Id = 2,
                    TipoCliente = ETipoCliente.PersonaFisica,
                    Nome = "carlo",
                    Cognome = "verdi",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = "",
                    Telefono = "2222",
                    Telefono2 = "1111",
                    Cellulare = "",
                    EMail = "gigi.verdi@gmail.com",
                    Email2 = "bcd@gmail.com"
                },
                new()
                {
                    Id = 3,
                    TipoCliente = ETipoCliente.PersonaFisica,
                    Nome = "gianluca",
                    Cognome = "rossi",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = "",
                    Telefono = "1111",
                    Telefono2 = "333/1222",
                    Cellulare = "",
                    EMail = "rossi@gmail.com",
                    Email2 = "bcd"
                },
                new()
                {
                    Id = 4,
                    TipoCliente = ETipoCliente.PersonaFisica,
                    Nome = "gianluca",
                    Cognome = "rossini",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = "",
                    Telefono = "",
                    Telefono2 = "",
                    Cellulare = "333-12225",
                    EMail = "11gian@gmail.com",
                    Email2 = ""
                },
                new()
                {
                    Id = 5,
                    TipoCliente = ETipoCliente.PersonaFisica,
                    Nome = "veronica",
                    Cognome = "carloni",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = "",
                    Telefono = @"333\12228",
                    Telefono2 = "",
                    Cellulare = "",
                    EMail = "",
                    Email2 = "11v@gmail.com"
                },
                new()
                {
                    Id = 6,
                    TipoCliente = ETipoCliente.PersonaFisica,
                    Nome = "denise",
                    Cognome = "vita",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = "",
                    Telefono = "",
                    Telefono2 = "",
                    Cellulare = "33312227",
                    EMail = "",
                    Email2 = ""
                },
                new()
                {
                    Id = 7,
                    TipoCliente = ETipoCliente.PersonaFisica,
                    Nome = "maria",
                    Cognome = "de vitalis",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = "",
                    Telefono = "",
                    Telefono2 = "",
                    Cellulare = "",
                    EMail = "",
                    Email2 = ""
                },
                new()
                {
                    Id = 8,
                    TipoCliente = ETipoCliente.PersonaFisica,
                    Nome = "stefano",
                    Cognome = "de vitalis",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = "",
                    Telefono = "",
                    Telefono2 = "",
                    Cellulare = "",
                    EMail = "",
                    Email2 = ""
                },
                new()
                {
                    Id = 9,
                    TipoCliente = ETipoCliente.PersonaFisica,
                    Nome = "stefano",
                    Cognome = "de vita",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = "",
                    Telefono = "",
                    Telefono2 = "",
                    Cellulare = "",
                    EMail = "",
                    Email2 = "giulia@alice.it"
                },
                new()
                {
                    Id = 10,
                    TipoCliente = ETipoCliente.PersonaFisica,
                    Nome = "stefano",
                    Cognome = "de sanctis",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = "gigi",
                    Telefono = "4444",
                    Telefono2 = "",
                    Cellulare = "11321",
                    EMail = "",
                    Email2 = "gino@gmail.com"
                },
                new()
                {
                    Id = 11,
                    TipoCliente = ETipoCliente.PersonaFisica,
                    Nome = "giulia 11",
                    Cognome = "rossi",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = "",
                    Telefono = "3333",
                    Telefono2 = "",
                    Cellulare = "",
                    EMail = "",
                    Email2 = "giulia11@outlook.com"
                },
                new()
                {
                    Id = 12,
                    TipoCliente = ETipoCliente.Societa,
                    Nome = "giulia 11",
                    Cognome = "rossi",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = "boh srl",
                    Telefono = "",
                    Telefono2 = "",
                    Cellulare = "",
                    EMail = "giulia@gmail.com",
                    Email2 = ""
                }
            };

            return clienti;
        }
    }
}
