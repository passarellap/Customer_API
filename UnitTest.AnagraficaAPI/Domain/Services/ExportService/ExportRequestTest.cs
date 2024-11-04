using AnagraficaAPI.Domain.Command;
using AnagraficaAPI.Domain.Services;
using DocsGeneratorService;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Poseidon.AuthorizationManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreePlat.Domain.Enumerations;

namespace ExportServiceTests
{
    [TestClass]
    public class ExportRequestTest
    {
        [TestMethod]
        public void GetExcelRequestTest()
        {
            ExportService exportService = GetExportService();

            AnagraficaFilterCommand command = new()
            {
                Nominativo = "a",
                Telefono = "333",
                EMail = "prova@gmail.com",
                TipoCliente = 1,
                NascitaData = new DateTime(1990, 1, 12),
                IdsTag = new List<int>() { 1, 2 },
                OrderBy = new List<string>() { "Id" } 
            };

            GetExcelFromQueryRequest response = exportService.GetExcelRequest(command);

            Assert.IsNotNull(response);

            Assert.IsTrue(response.QueryObject.FilterList.Any(x => x.Field == "Nominativo"));
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "Nominativo").Select(x => x.Value).First(), "a");
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "Nominativo").Select(x => x.FilterOperator).First(), 1);

            Assert.IsTrue(response.QueryObject.FilterList.Any(x => x.Field == "Telefono"));
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "Telefono").Select(x => x.Value).First(), "333");
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "Telefono").Select(x => x.FilterOperator).First(), 1);

            Assert.IsTrue(response.QueryObject.FilterList.Any(x => x.Field == "EMail"));
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "EMail").Select(x => x.Value).First(), "prova@gmail.com");
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "EMail").Select(x => x.FilterOperator).First(), 1);

            Assert.IsTrue(response.QueryObject.FilterList.Any(x => x.Field == "TipoCliente"));
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "TipoCliente").Select(x => x.Value).First(), "1");
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "TipoCliente").Select(x => x.FilterOperator).First(), 1);

            Assert.IsTrue(response.QueryObject.FilterList.Any(x => x.Field == "NascitaData"));
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "NascitaData").Select(x => x.Value).First(), "1990/01/12 00:00:00");
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "NascitaData").Select(x => x.FilterOperator).First(), 5);

            Assert.IsTrue(response.QueryObject.FilterList.Any(x => x.Field == "IDTag"));
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "IDTag").Select(x => x.Value).First(), "1");
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "IDTag").Select(x => x.FilterOperator).First(), 1);
            Assert.IsTrue(response.QueryObject.FilterList.Where(x => x.Field == "IDTag").Any(x => x.SubFilterList.Length > 0));
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "IDTag").Select(x => x.SubFilterList.First().Value).First(), "2");

            Assert.IsTrue(response.QueryObject.FilterList.Any(x => x.Field == "IDGestione"));
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "IDGestione").Select(x => x.Value).First(), "543");
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "IDGestione").Select(x => x.FilterOperator).First(), 1);

            Assert.IsTrue(response.QueryObject.FilterList.Any(x => x.Field == "Stato"));
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "Stato").Select(x => x.Value).First(), "1");
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "Stato").Select(x => x.FilterOperator).First(), 1);
        }

        [TestMethod]
        public void GetExcelRequestWithoutFiltersTest()
        {
            ExportService exportService = GetExportService();

            AnagraficaFilterCommand command = new();

            GetExcelFromQueryRequest response = exportService.GetExcelRequest(command);

            Assert.IsNotNull(response);

            Assert.IsTrue(!response.QueryObject.FilterList.Any(x => x.Field == "Nominativo"));

            Assert.IsTrue(!response.QueryObject.FilterList.Any(x => x.Field == "Telefono"));

            Assert.IsTrue(!response.QueryObject.FilterList.Any(x => x.Field == "EMail"));

            Assert.IsTrue(!response.QueryObject.FilterList.Any(x => x.Field == "TipoCliente"));

            Assert.IsTrue(!response.QueryObject.FilterList.Any(x => x.Field == "NascitaData"));

            Assert.IsTrue(!response.QueryObject.FilterList.Any(x => x.Field == "IDTag"));

            Assert.IsTrue(response.QueryObject.FilterList.Any(x => x.Field == "IDGestione"));
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "IDGestione").Select(x => x.Value).First(), "543");
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "IDGestione").Select(x => x.FilterOperator).First(), 1);

            Assert.IsTrue(response.QueryObject.FilterList.Any(x => x.Field == "Stato"));
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "Stato").Select(x => x.Value).First(), "1");
            Assert.AreEqual(response.QueryObject.FilterList.Where(x => x.Field == "Stato").Select(x => x.FilterOperator).First(), 1);
        }

        private static ExportService GetExportService()
        {
            Mock<ILogger<ExportService>> mockLogger = new();
            Mock<IAuthorizationManager> mockAutho = new();
            Mock<IDocService> mockDocService = new();
            mockAutho.Setup(x => x.HasAuthorization(It.IsAny<EPrivilegioPoseidon>())).Returns(true);
            mockAutho.Setup(x => x.IdAgenzia).Returns(543);

            return new ExportService(mockLogger.Object, mockAutho.Object, mockDocService.Object);
        }
    }
}
