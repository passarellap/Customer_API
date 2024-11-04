using AnagraficaAPI.Domain.Command;
using AnagraficaAPI.Domain.Model;
using AnagraficaAPI.Domain.Services;
using AnagraficaAPI.Entity.Context;
using AnagraficaAPI.Entity.Model;
using AutoMapper;
using DocsGeneratorService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using Poseidon.AuthorizationManager;
using ServiceUtilities.Model;
using ServiceUtilities.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreePlat.Domain.Enumerations;
using UnitTest.AnagraficaAPI;

namespace AnagraficaServiceTest
{
    [TestClass]
    public class AnagraficaGetTest
    {
        private List<Cliente> entities;

        [TestInitialize]
        public void TestInit()
        {
            entities = MockDatabase.GetClientiList();
        }

        // GetListByFilter
        [TestMethod]
        public async Task GetFilterTest_Good()
        {
            Mock<IMapper> mockMapper = new();

            List<Cliente> clienti = new()
            {
                new()
                {
                    Id = 1,
                    Stato = EStatoCliente.Attivo,
                    Nome = "nome1",
                    Cognome = "cognome1",
                    RagioneSociale = "",
                    Cellulare = "341233323123",
                    EMail = "mail.nome1@mail.com",
                    TipoCliente = ETipoCliente.PersonaFisica

                },
                new()
                {
                    Id = 2,
                    Stato = EStatoCliente.Attivo,
                    Nome = "nome2",
                    Cognome = "cognome2",
                    RagioneSociale = "",
                    Cellulare = "3331234444",
                    EMail = "mail.nome2@mail.com",
                    TipoCliente = ETipoCliente.PersonaFisica
                },
                new()
                {
                    Id = 4,
                    Stato = EStatoCliente.Attivo,
                    Nome = "",
                    Cognome = "",
                    RagioneSociale = "RagioneSociale nomeABC",
                    Cellulare = "32321233232",
                    EMail = "mail.ragione.sociale@mail.com",
                    TipoCliente = ETipoCliente.Societa
                },
                new()
                {
                    Id = 5,
                    Stato = EStatoCliente.Eliminato,
                    Nome = "massimo",
                    Cognome = "rossio",
                    RagioneSociale = "",
                    Cellulare = "56567568",
                    EMail = "mail.nome4@mail.com",
                    TipoCliente = ETipoCliente.PersonaFisica
                }
            };

            IAnagraficaService service = GetAnagraficaService(mockMapper, clienti);

            AnagraficaFilterCommand filter = new()
            {
                Nominativo = "nome",
                Telefono = "123",
                EMail = "mail",
                TipoCliente = (int)ETipoCliente.PersonaFisica,
                OrderBy = new List<string>() { "Nome", "Cognome", "RagioneSociale" },
                PageNumber = 0,
                PageSize = 2
            };

            PagedViewModel<AnagraficaModel> pagedList = await service.GetListByFilter(filter);

            mockMapper.Verify(x => x.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.IsAny<PagedList<Cliente>>()), Times.Once());
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Count == 2)), "Totale numero di Records errato, atteso 2");

            int id = 1;
            string prefix = $"Record id={id}";
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).Any())), $"{prefix} non presente");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().Nome.Equals("nome1"))), $"{prefix} Nome errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().Cognome.Equals("cognome1"))), $"{prefix} Cognome errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().RagioneSociale.Equals(""))), $"{prefix} RagioneSociale errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().Cellulare.Equals("341233323123"))), $"{prefix} Cellulare errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().EMail.Equals("mail.nome1@mail.com"))), $"{prefix} EMail errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().TipoCliente == ETipoCliente.PersonaFisica)), $"{prefix} TipoCliente errato");

            id = 2;
            prefix = $"Record id={id}";
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).Any())), $"{prefix} non presente");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().Nome.Equals("nome2"))), $"{prefix} Nome errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().Cognome.Equals("cognome2"))), $"{prefix} Cognome errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().RagioneSociale.Equals(""))), $"{prefix} RagioneSociale errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().Cellulare.Equals("3331234444"))), $"{prefix} Cellulare errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().EMail.Equals("mail.nome2@mail.com"))), $"{prefix} EMail errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().TipoCliente == ETipoCliente.PersonaFisica)), $"{prefix} TipoCliente errato");
        }

        [TestMethod]
        public async Task GetOne_Good()
        {
            Mock<IMapper> mockMapper = new();
            
            Cliente cliente1 = new()
            {
                Id = 1,
                IdAgenzia = 1,
                IdCollaboratore = 2,
                IdIndirizzo = 3,
                Stato = EStatoCliente.Attivo,
                StatoCivile = EStatoCivile.Coniugato,
                Nome = "nome1",
                Cognome = "cognome1",
                RagioneSociale = "ragionesociale1",
                Telefono = "02234234233",
                Telefono2 = "0209877443",
                Cellulare = "340123123123",
                Fax = "022348492389",
                EMail = "mario.rossi@mail.it",
                Email2 = "mario.rossi@mail2.it",
                SitoInternet = "SitoInternet1",
                SkypeId = "SkypeId1",
                AltriRecapiti = "AltriRecapiti1",
                TipoCanalePreferito = ETipoCanalePreferito.Email,
                Note = "Note1",
                TipoConsenso = 7,
                IdEditore = 8,
                TipoCliente = ETipoCliente.PersonaFisica,
                TipoImpiego = ETipoImpiego.ImpPrivato,
                TipoSesso = ETipoSesso.Maschio,
                TipoTitolo = ETipoTitolo.Dr,
                IdSettore = 13,
                AutoritaLuogoRilascio = "AutoritaLuogoRilascio1",
                CodiceCRM = "CodiceCRM1",
                CodiceFiscale = "CodiceFiscale1",
                CodicePadre = 14,
                CodicePadreCrm = "CodicePadreCrm1",
                DataRilascioDocumento = new DateTime(2019, 10, 15),
                DataScadenzaDocumento = new DateTime(2019, 11, 15),
                NumeroDocumentoPresentato = "NumeroDocumentoPresentato1",
                Piva = "Piva1",
                TagCliente = 15,
                TipoDocumentoPresentato = ETipoDocumentoPresentato.Cartadiidentita,
                NascitaLuogo = "NascitaLuogo1",
                NascitaProvincia = "NascitaProvincia1",
                NascitaData = new DateTime(2019, 12, 15),
                EsenteAui = true,
                RowVersion = new byte[8] { 90, 91, 92, 93, 99, 98, 92, 97 }
            };
            List<Cliente> clienti = new() { cliente1, new Cliente() { Id = 2 }, new Cliente() { Id = 3 } };

            IAnagraficaService service = GetAnagraficaService(mockMapper, clienti);
            _ = await service.Get(1);

            mockMapper.Verify(x => x.Map<AnagraficaModel>(It.IsAny<Cliente>()), Times.Once());
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.Id == 1)), $"Record non trovato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.IdCollaboratore == 2)), $"IdCollaboratore errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.IdIndirizzo == 3)), $"Idindirizzo errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.Stato == EStatoCliente.Attivo)), $"Stato errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.StatoCivile == EStatoCivile.Coniugato)), $"StatoCivile errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.Nome.Equals("nome1"))), $"Nome errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.Cognome.Equals("cognome1"))), $"Cognome errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.RagioneSociale.Equals("ragionesociale1"))), $"RagioneSociale errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.Telefono.Equals("02234234233"))), $"Telefono errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.Telefono2.Equals("0209877443"))), $"Telefono2 errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.Cellulare.Equals("340123123123"))), $"Cellulare errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.Fax.Equals("022348492389"))), $"Fax errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.EMail.Equals("mario.rossi@mail.it"))), $"EMail errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.Email2.Equals("mario.rossi@mail2.it"))), $"Email2 errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.SitoInternet.Equals("SitoInternet1"))), $"SitoInternet errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.SkypeId.Equals("SkypeId1"))), $"SkypeId errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.AltriRecapiti.Equals("AltriRecapiti1"))), $"AltriRecapiti errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.TipoCanalePreferito == ETipoCanalePreferito.Email)), $"TipoCanalePreferito errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.Note.Equals("Note1"))), $"Note errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.TipoConsenso == 7)), $"TipoConsenso errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.IdEditore == 8)), $"Ideditore errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.TipoCliente == ETipoCliente.PersonaFisica)), $"TipoCliente errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.TipoImpiego == ETipoImpiego.ImpPrivato)), $"TipoImpiego errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.TipoSesso == ETipoSesso.Maschio)), $"TipoSesso errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.TipoTitolo == ETipoTitolo.Dr)), $"TipoTitolo errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.IdSettore == 13)), $"Idsettore errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.AutoritaLuogoRilascio.Equals("AutoritaLuogoRilascio1"))), $"AutoritaLuogoRilascio errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.CodiceCRM.Equals("CodiceCRM1"))), $"CodiceCRM errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.CodiceFiscale.Equals("CodiceFiscale1"))), $"CodiceFiscale errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.CodicePadre == 14)), $"CodicePadre errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.DataRilascioDocumento.Equals(new DateTime(2019, 10, 15)))), $"DataRilascioDocumento errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.DataScadenzaDocumento.Equals(new DateTime(2019, 11, 15)))), $"DataScadenzaDocumento errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.NumeroDocumentoPresentato.Equals("NumeroDocumentoPresentato1"))), $"NumeroDocumentoPresentato errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.Piva.Equals("Piva1"))), $"Piva errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.TagCliente == 15)), $"TagCliente errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.TipoDocumentoPresentato == ETipoDocumentoPresentato.Cartadiidentita)), $"TipoDocumentoPresentato errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.NascitaLuogo.Equals("NascitaLuogo1"))), $"NascitaLuogo errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.NascitaProvincia.Equals("NascitaProvincia1"))), $"NascitaProvincia errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.NascitaData.Equals(new DateTime(2019, 12, 15)))), $"NascitaData errato");
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(x => x.EsenteAui == true)), $"EsenteAui errato");
        }

        [TestMethod]
        public async Task GetOne_NotFound()
        {
            Mock<IMapper> mockMapper = new();
           
            List<Cliente> clienti = new() { new Cliente() { Id = 1 }, new Cliente() { Id = 2 }, new Cliente() { Id = 3 } };

            IAnagraficaService service = GetAnagraficaService(mockMapper, clienti);
            _ = await service.Get(4);

            mockMapper.Verify(x => x.Map<AnagraficaModel>(It.IsAny<Cliente>()), Times.Once());
            mockMapper.Verify(m => m.Map<AnagraficaModel>(It.Is<Cliente>(l => l == null)), "Record trovato ma non avrebbe dovuto");
        }

        [TestMethod]
        public async Task GetSuggestionByNominativo()
        {
            Mock<IMapper> mockMapper = new();

            List<Cliente> clienti = new()
            {
                new()
                {
                    Id = 1,
                    Stato = EStatoCliente.Attivo,
                    Nome = "Aldo",
                    Cognome = "Bassi",
                    RagioneSociale = "",
                    Cellulare = "341233323123",
                    EMail = "mail.nome1@mail.com",
                    TipoCliente = ETipoCliente.PersonaFisica

                },
                new()
                {
                    Id = 2,
                    Stato = EStatoCliente.Attivo,
                    Nome = "Osvaldo",
                    Cognome = "Rossi",
                    RagioneSociale = "",
                    Cellulare = "3331234444",
                    EMail = "mail.nome2@mail.com",
                    TipoCliente = ETipoCliente.PersonaFisica
                },
                new()
                {
                    Id = 4,
                    Stato = EStatoCliente.Attivo,
                    Nome = "Anna",
                    Cognome = "Rossi",
                    RagioneSociale = "Anna",
                    Cellulare = "32321233232",
                    EMail = "mail.ragione.sociale@mail.com",
                    TipoCliente = ETipoCliente.Societa
                }
            };

            IAnagraficaService service = GetAnagraficaService(mockMapper, clienti);

            AnagraficaFilterCommand filter = new()
            {
                Nominativo = "a oss",
                OrderBy = new List<string> { "Id" },
            };

            PagedViewModel<AnagraficaModel> result = await service.GetListByFilter(filter);

            mockMapper.Verify(x => x.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.IsAny<PagedList<Cliente>>()), Times.Once());
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Count == 1)), "Totale numero di Records errato, atteso 1");

            int id = 2;
            string prefix = $"Record id={id}";
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).Any())), $"{prefix} non presente");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().Nome.Equals("Osvaldo"))), $"{prefix} Nome errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().Cognome.Equals("Rossi"))), $"{prefix} Cognome errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().RagioneSociale.Equals(""))), $"{prefix} RagioneSociale errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().Cellulare.Equals("3331234444"))), $"{prefix} Cellulare errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().EMail.Equals("mail.nome2@mail.com"))), $"{prefix} EMail errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().TipoCliente == ETipoCliente.PersonaFisica)), $"{prefix} TipoCliente errato");

        }

        [TestMethod]
        public async Task GetSuggestionRagioneSocialeByNominativo()
        {
            Mock<IMapper> mockMapper = new();

            List<Cliente> clienti = new()
            {
                new()
                {
                    Id = 1,
                    Stato = EStatoCliente.Attivo,
                    Nome = "Aldo",
                    Cognome = "Bassi",
                    RagioneSociale = "Bassi",
                    TipoCliente = ETipoCliente.PersonaFisica

                },
                new()
                {
                    Id = 2,
                    Stato = EStatoCliente.Attivo,
                    Nome = "Osvaldo",
                    Cognome = "Rossi",
                    RagioneSociale = "",
                    TipoCliente = ETipoCliente.PersonaFisica
                },
                new()
                {
                    Id = 4,
                    Stato = EStatoCliente.Attivo,
                    Nome = "Anna",
                    Cognome = "Rossi",
                    RagioneSociale = "Bassini",
                    TipoCliente = ETipoCliente.Societa
                }
            };

            IAnagraficaService service = GetAnagraficaService(mockMapper, clienti);

            AnagraficaFilterCommand filter = new()
            {
                Nominativo = "ssi",
                OrderBy = new List<string> { "Id" },
            };

            PagedViewModel<AnagraficaModel> result = await service.GetListByFilter(filter);

            mockMapper.Verify(x => x.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.IsAny<PagedList<Cliente>>()), Times.Once());
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Count == 3)), "Totale numero di Records errato, atteso 3");

            int id = 1;
            string prefix = $"Record id={id}";
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).Any())), $"{prefix} non presente");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().Nome.Equals("Aldo"))), $"{prefix} Nome errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().Cognome.Equals("Bassi"))), $"{prefix} Cognome errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().RagioneSociale.Equals("Bassi"))), $"{prefix} RagioneSociale errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().TipoCliente == ETipoCliente.PersonaFisica)), $"{prefix} TipoCliente errato");

            id = 2;
            prefix = $"Record id={id}";
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).Any())), $"{prefix} non presente");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().Nome.Equals("Osvaldo"))), $"{prefix} Nome errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().Cognome.Equals("Rossi"))), $"{prefix} Cognome errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().RagioneSociale.Equals(""))), $"{prefix} RagioneSociale errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().TipoCliente == ETipoCliente.PersonaFisica)), $"{prefix} TipoCliente errato");

            id = 4;
            prefix = $"Record id={id}";
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).Any())), $"{prefix} non presente");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().Nome.Equals("Anna"))), $"{prefix} Nome errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().Cognome.Equals("Rossi"))), $"{prefix} Cognome errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().RagioneSociale.Equals("Bassini"))), $"{prefix} RagioneSociale errato");
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).FirstOrDefault().TipoCliente == ETipoCliente.Societa)), $"{prefix} TipoCliente errato");
        }


        [TestMethod]
        public async Task GetByNominativo()
        {
            Mock<IMapper> mockMapper = new();

            IAnagraficaService anagraficaService = GetAnagraficaService(mockMapper, entities);

            AnagraficaFilterCommand command = new() { Nominativo = "de vi", OrderBy = new List<string>() { "Id" } };

            var response = await anagraficaService.GetListByFilter(command);

            mockMapper.Verify(x => x.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.IsAny<PagedList<Cliente>>()), Times.Once());
            mockMapper.VerifyCount(4);
            mockMapper.VerifyRecord(6);
            mockMapper.VerifyRecord(7);
            mockMapper.VerifyRecord(8);
            mockMapper.VerifyRecord(9);
        }

        [TestMethod]
        public async Task GetListByMultipleNominativo()
        {
            Mock<IMapper> mockMapper = new();

            IAnagraficaService anagraficaService = GetAnagraficaService(mockMapper, entities);

            AnagraficaFilterCommand command = new() { Nominativo = "ste de vi", OrderBy = new List<string>() { "Id" } };
            _ = await anagraficaService.GetListByFilter(command);

            mockMapper.VerifyCount(2);
            mockMapper.VerifyRecord(8);
            mockMapper.VerifyRecord(9);
        }

        [TestMethod]
        public async Task GetByFreeTextNumber()
        {
            Mock<IMapper> mockMapper = new();

            IAnagraficaService anagraficaService = GetAnagraficaService(mockMapper, entities);

            AnagraficaFilterCommand command = new() { FreeText = "11", OrderBy = new List<string>() { "Id" } };

            var response = await anagraficaService.GetListByFilter(command);

            mockMapper.Verify(x => x.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.IsAny<PagedList<Cliente>>()), Times.Once());
            mockMapper.VerifyCount(6);
            mockMapper.VerifyRecord(2);
            mockMapper.VerifyRecord(3);
            mockMapper.VerifyRecord(4);
            mockMapper.VerifyRecord(5);
            mockMapper.VerifyRecord(10);
            mockMapper.VerifyRecord(11);
        }

        [TestMethod]
        public async Task GetByFreeTextString()
        {
            Mock<IMapper> mockMapper = new();

            IAnagraficaService anagraficaService = GetAnagraficaService(mockMapper, entities);

            AnagraficaFilterCommand command = new() { FreeText = "gi", OrderBy = new List<string>() { "Id" } };

            var response = await anagraficaService.GetListByFilter(command);

            mockMapper.Verify(x => x.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.IsAny<PagedList<Cliente>>()), Times.Once());
            mockMapper.VerifyCount(8);
            mockMapper.VerifyRecord(1);
            mockMapper.VerifyRecord(2);
            mockMapper.VerifyRecord(3);
            mockMapper.VerifyRecord(4);
            mockMapper.VerifyRecord(9);
            mockMapper.VerifyRecord(10);
            mockMapper.VerifyRecord(11);
            mockMapper.VerifyRecord(12);
        }

        [TestMethod]
        public async Task GetByFreeTextMultiple()
        {
            Mock<IMapper> mockMapper = new();

            IAnagraficaService anagraficaService = GetAnagraficaService(mockMapper, entities);

            AnagraficaFilterCommand command = new() { FreeText = "ste de vi", OrderBy = new List<string>() { "Id" } };

            _ = await anagraficaService.GetListByFilter(command);

            mockMapper.VerifyCount(2);
            mockMapper.VerifyRecord(8);
            mockMapper.VerifyRecord(9);
        }

        [TestMethod]
        public async Task GetByEmail()
        {
            Mock<IMapper> mockMapper = new();

            IAnagraficaService anagraficaService = GetAnagraficaService(mockMapper, entities);

            AnagraficaFilterCommand command = new() { EMail = "giuli", OrderBy = new List<string>() { "Id" } };

            _ = await anagraficaService.GetListByFilter(command);

            mockMapper.VerifyCount(3);
            mockMapper.VerifyRecord(9);
            mockMapper.VerifyRecord(11);
            mockMapper.VerifyRecord(12);
        }

        //GetListSortByNominativo
        [TestMethod]
        public async Task GetListSortByFreeText()
        {
            Mock<IMapper> mockMapper = new();

            IAnagraficaService anagraficaService = GetAnagraficaService(mockMapper, entities);

            _ = await anagraficaService.GetListSortByFreeText("carlo verd");
            mockMapper.VerifyListCount(2);
            mockMapper.VerifyListRecord(1);
            mockMapper.VerifyListRecordOrder(1, 1);
            mockMapper.VerifyListRecord(2);
            mockMapper.VerifyListRecordOrder(2, 0);
        }

        [TestMethod]
        public async Task GetListSortByMultipleNominativo()
        {
            Mock<IMapper> mockMapper = new();

            IAnagraficaService anagraficaService = GetAnagraficaService(mockMapper, entities);

            _ = await anagraficaService.GetListSortByFreeText("stefano de vita");

            mockMapper.VerifyListCount(2);
            mockMapper.VerifyListRecord(8);
            mockMapper.VerifyListRecordOrder(8, 1);
            mockMapper.VerifyListRecord(9);
            mockMapper.VerifyListRecordOrder(9, 0);
        }

        [TestMethod]
        public async Task GetListSortByNumber()
        {
            Mock<IMapper> mockMapper = new();

            IAnagraficaService anagraficaService = GetAnagraficaService(mockMapper, entities);

            _ = await anagraficaService.GetListSortByFreeText("3331222");

            mockMapper.VerifyListCount(5);
            mockMapper.VerifyListRecord(1);
            mockMapper.VerifyListRecordOrder(1, 4);
            mockMapper.VerifyListRecord(3);
            mockMapper.VerifyListRecordOrder(3, 0);
            mockMapper.VerifyListRecord(4);
            mockMapper.VerifyListRecordOrder(4, 1);
            mockMapper.VerifyListRecord(5);
            mockMapper.VerifyListRecordOrder(5, 2);
            mockMapper.VerifyListRecord(6);
            mockMapper.VerifyListRecordOrder(6, 3);
        }

        [TestMethod]
        public async Task GetListSortByMail()
        {
            Mock<IMapper> mockMapper = new();

            IAnagraficaService anagraficaService = GetAnagraficaService(mockMapper, entities);

            _ = await anagraficaService.GetListSortByFreeText("bcd");

            mockMapper.VerifyListCount(3);
            mockMapper.VerifyListRecord(1);
            mockMapper.VerifyListRecordOrder(1, 2);
            mockMapper.VerifyListRecord(2);
            mockMapper.VerifyListRecordOrder(2, 1);
            mockMapper.VerifyListRecord(3);
            mockMapper.VerifyListRecordOrder(3, 0);
        }

        private static AnagraficaService GetAnagraficaService(Mock<IMapper> mockMapper, List<Cliente> clienti)
        {
            IAnagraficaCustomFilter customFilter = new AnagraficaCustomFilter();

            Mock<ILogger<AnagraficaService>> mockLogger = new();
            Mock<IAuthorizationManager> mockFilter = new();
            Mock<AnagraficaContext> mockContext = new();
            Mock<DbSet<Cliente>> mockDbset = new();
            mockDbset = clienti.AsQueryable().BuildMockDbSet();
            mockContext.SetupGet(x => x.Clienti).Returns(mockDbset.Object);

            return new AnagraficaService(mockContext.Object, mockLogger.Object, mockMapper.Object, customFilter, mockFilter.Object);
        }
    }
}

