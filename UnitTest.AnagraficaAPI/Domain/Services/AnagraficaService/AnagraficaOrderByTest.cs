using AnagraficaAPI.Domain.Command;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreePlat.Domain.Enumerations;
using UnitTest.AnagraficaAPI;

namespace AnagraficaServiceTest
{
    [TestClass]
    public class AnagraficaOrderByTest
    {
        private List<Cliente> entities;

        [TestInitialize]
        public void TestInit()
        {
            entities = new List<Cliente>()
            {
                new Cliente()
                {
                    Id = 1,
                    TipoCliente = ETipoCliente.PersonaFisica,
                    Nome = "giancarlo",
                    Cognome = "verdi",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = ""
                },
                new Cliente()
                {
                    Id = 2,
                    TipoCliente = ETipoCliente.Societa,
                    Nome = "carlo",
                    Cognome = "verdi",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = "società srl"
                },
                new Cliente()
                {
                    Id = 3,
                    TipoCliente = ETipoCliente.PersonaFisica,
                    Nome = "gianluca",
                    Cognome = "rossi",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = ""
                },
                new Cliente()
                {
                    Id = 4,
                    TipoCliente = ETipoCliente.PersonaFisica,
                    Nome = "gianluca",
                    Cognome = "prova",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = ""
                },
                new Cliente()
                {
                    Id = 5,
                    TipoCliente = ETipoCliente.Societa,
                    Nome = "",
                    Cognome = "",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = "azienda"
                }
                ,
                new Cliente()
                {
                    Id = 6,
                    TipoCliente = ETipoCliente.PersonaFisica,
                    Nome = "nino",
                    Cognome = "verdi",
                    Stato = EStatoCliente.Attivo,
                    RagioneSociale = ""
                }
            };
        }

        [TestMethod]
        public async Task OrderByNome()
        {
            Mock<IMapper> mockMapper = new();

            IAnagraficaService anagraficaService = GetAnagraficaService(mockMapper, entities);

            AnagraficaFilterCommand command = new() { OrderBy = new List<string>() { "Nome" } };

            _ = await anagraficaService.GetListByFilter(command);

            mockMapper.VerifyCount(6);
            mockMapper.VerifyRecordOrder(1, 1);
            mockMapper.VerifyRecordOrder(2, 5);
            mockMapper.VerifyRecordOrder(3, 3);
            mockMapper.VerifyRecordOrder(4, 2);
            mockMapper.VerifyRecordOrder(5, 0);
            mockMapper.VerifyRecordOrder(6, 4);

            command.OrderBy = new List<string>() { "Nome_desc" };

            _ = await anagraficaService.GetListByFilter(command);

            mockMapper.VerifyCount(6);
            mockMapper.VerifyRecordOrder(1, 4);
            mockMapper.VerifyRecordOrder(2, 0);
            mockMapper.VerifyRecordOrder(3, 2);
            mockMapper.VerifyRecordOrder(4, 3);
            mockMapper.VerifyRecordOrder(5, 5);
            mockMapper.VerifyRecordOrder(6, 1);

        }

        [TestMethod]
        public async Task OrderByCognome()
        {
            Mock<IMapper> mockMapper = new();

            IAnagraficaService anagraficaService = GetAnagraficaService(mockMapper, entities);

            AnagraficaFilterCommand command = new() { OrderBy = new List<string>() { "Cognome" } };

            _ = await anagraficaService.GetListByFilter(command);

            mockMapper.VerifyCount(6);
            mockMapper.VerifyRecordOrder(1, 4);
            mockMapper.VerifyRecordOrder(2, 3);
            mockMapper.VerifyRecordOrder(3, 2);
            mockMapper.VerifyRecordOrder(4, 1);
            mockMapper.VerifyRecordOrder(5, 0);
            mockMapper.VerifyRecordOrder(6, 5);

            command.OrderBy = new List<string>() { "Cognome_desc" };

            _ = await anagraficaService.GetListByFilter(command);

            mockMapper.VerifyCount(6);
            mockMapper.VerifyRecordOrder(1, 1);
            mockMapper.VerifyRecordOrder(2, 2);
            mockMapper.VerifyRecordOrder(3, 3);
            mockMapper.VerifyRecordOrder(4, 4);
            mockMapper.VerifyRecordOrder(5, 5);
            mockMapper.VerifyRecordOrder(6, 0);
        }

        [TestMethod]
        public async Task OrderById()
        {
            Mock<IMapper> mockMapper = new();

            IAnagraficaService anagraficaService = GetAnagraficaService(mockMapper, entities);

            AnagraficaFilterCommand command = new() { OrderBy = new List<string>() { "Id_desc" } };

            _ = await anagraficaService.GetListByFilter(command);

            mockMapper.VerifyCount(6);
            mockMapper.VerifyRecordOrder(1, 5);
            mockMapper.VerifyRecordOrder(2, 4);
            mockMapper.VerifyRecordOrder(3, 3);
            mockMapper.VerifyRecordOrder(4, 2);
            mockMapper.VerifyRecordOrder(5, 1);
            mockMapper.VerifyRecordOrder(6, 0);
        }

        [TestMethod]
        public async Task VoidOrderBy()
        {
            Mock<IMapper> mockMapper = new();

            IAnagraficaService anagraficaService = GetAnagraficaService(mockMapper, entities);

            AnagraficaFilterCommand command = new() { OrderBy = new List<string>()};

            _ = await anagraficaService.GetListByFilter(command);

            mockMapper.VerifyCount(6);
            mockMapper.VerifyRecordOrder(1, 0);
            mockMapper.VerifyRecordOrder(2, 1);
            mockMapper.VerifyRecordOrder(3, 2);
            mockMapper.VerifyRecordOrder(4, 3);
            mockMapper.VerifyRecordOrder(5, 4);
            mockMapper.VerifyRecordOrder(6, 5);
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
