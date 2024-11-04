using AnagraficaAPI.Domain.Command;
using AnagraficaAPI.Domain.Services;
using AnagraficaAPI.Entity.Context;
using AnagraficaAPI.Entity.Model;
using AutoMapper;
using DocsGeneratorService;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Poseidon.AuthorizationManager;
using ServiceUtilities.Model;
using System;
using System.Threading;
using System.Threading.Tasks;
using TreePlat.Domain.Enumerations;

namespace AnagraficaServiceTest
{
    [TestClass]
    public class AnagraficaAddTest
    {
        [TestMethod]
        public async Task AddZonaTest_Good()
        {
            Mock<AnagraficaContext> mockContext = new();
            Mock<IMapper> mockMapper = new();
            Mock<IAnagraficaCustomFilter> mockFilter = new();
            Mock<IAuthorizationManager> mockAuthorization = new();
            Mock<ILogger<AnagraficaService>> moqLogger = new();

            mockAuthorization.Setup(x => x.HasAuthorization(It.IsAny<EPrivilegioPoseidon>())).Returns(true);
            mockAuthorization.Setup(x => x.IdAgenzia).Returns(543);
            mockAuthorization.Setup(x => x.IdCollaboratore).Returns(1435);
            mockContext.Setup(_ => _.AddAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>())).Verifiable();
            mockContext.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1).Verifiable();
            mockMapper.Setup(x => x.Map(It.IsAny<AnagraficaSaveCommand>(), It.IsAny<Cliente>())).Returns(new Cliente() { });

            IAnagraficaService service = new AnagraficaService(mockContext.Object, moqLogger.Object, mockMapper.Object, mockFilter.Object, mockAuthorization.Object);
            UpdatedModel updatedModel = await service.Insert(new AnagraficaSaveCommand());

            Assert.IsNull(updatedModel);
            mockContext.Verify();
            mockContext.Verify(x => x.AddAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()), Times.Once());
            mockContext.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task AddZonaTest_Wrong_Parameter()
        {
            Mock<IAuthorizationManager> mockAuthorization = new();
            Mock<ILogger<AnagraficaService>> moqLogger = new();

            mockAuthorization.Setup(x => x.HasAuthorization(It.IsAny<EPrivilegioPoseidon>())).Returns(true);

            IAnagraficaService service = new AnagraficaService(null, moqLogger.Object, null, null, mockAuthorization.Object);

            _ = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await service.Insert(null));
        }
    }
}