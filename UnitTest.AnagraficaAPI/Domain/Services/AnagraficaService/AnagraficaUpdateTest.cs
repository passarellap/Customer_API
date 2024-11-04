using AnagraficaAPI.Domain.Command;
using AnagraficaAPI.Domain.Services;
using AnagraficaAPI.Entity.Context;
using AnagraficaAPI.Entity.Model;
using AutoMapper;
using DocsGeneratorService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using Poseidon.AuthorizationManager;
using ServiceUtilities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TreePlat.Domain.Constants;
using TreePlat.Domain.Enumerations;

namespace AnagraficaServiceTest
{
    [TestClass]
    public class AnagraficaUpdateTest
    {
        [TestMethod]
        public async Task UpdateTest_Good()
        {
            Mock<AnagraficaContext> mockContext = new();
            Mock<IMapper> mockMapper = new();
            Mock<IAnagraficaCustomFilter> mockFilter = new();
            Mock<IAuthorizationManager> mockAuthorization = new();

            List<Cliente> clienti = new() { new Cliente() { Id = 1 }, new Cliente() { Id = 2 } };
            mockContext.SetupGet(c => c.Clienti).Returns(clienti.AsQueryable().BuildMockDbSet().Object);
            mockContext.Setup(x => x.Clienti.FindAsync(It.IsAny<int>())).ReturnsAsync(clienti.AsQueryable().BuildMockDbSet().Object.SingleOrDefault(x => x.Id == 1));
            mockAuthorization.Setup(x => x.HasAuthorization(It.IsAny<EPrivilegioPoseidon>())).Returns(true);

            mockContext.Setup(x => x.SetRowVersionValue(It.IsAny<Cliente>(), It.IsAny<byte[]>())).Verifiable();
            mockContext.Setup(x => x.SaveChangesAsync()).Verifiable();
            mockMapper.Setup(x => x.Map(It.IsAny<AnagraficaSaveCommand>(), It.IsAny<Cliente>())).Returns(new Cliente() { });

            IAnagraficaService service = new AnagraficaService(mockContext.Object, null, mockMapper.Object, mockFilter.Object, mockAuthorization.Object);
            AnagraficaSaveCommand anagraficaUpdate = new()
            {
                IdCollaboratore = 2,
                IdIndirizzo = 3,
                StatoCivile = EStatoCivile.Coniugato,
                Nome = "nome1",
                Cognome = "cognome1",
                RagioneSociale = "ragionesociale1",
                Telefono = "02234234233",
                Telefono2 = "0209877443",
                Cellulare = "340123123123",
                Fax = "022348492389",
                EMail = "mario.rossi@mail.it",
                EMail2 = "mario.rossi@mail2.it",
                SitoInternet = "SitoInternet1",
                SkypeId = "SkypeId1",
                AltriRecapiti = "AltriRecapiti1",
                TipoCanalePreferito = 6,
                Note = "Note1",
                TipoConsenso = 7,
                IdEditore = 8,
                TipoCliente = ETipoCliente.PersonaFisica,
                TipoImpiego = ETipoImpiego.ImpPrivato,
                TipoSesso = ETipoSesso.Femmina,
                TipoTitolo = ETipoTitolo.Avv,
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
                Idorigine = "Idorigine1",
                NascitaLuogo = "NascitaLuogo1",
                NascitaProvincia = "NascitaProvincia1",
                NascitaData = new DateTime(2019, 12, 15),
                EsenteAui = true,
                RowVersion = new byte[8] { 90, 91, 92, 93, 99, 98, 92, 97 }
            };
            UpdatedModel updatedModel = await service.Update(1, anagraficaUpdate);

            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.IdCollaboratore == 2), It.IsAny<Cliente>()), $"IdCollaboratore errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.IdIndirizzo == 3), It.IsAny<Cliente>()), $"Idindirizzo errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.StatoCivile == EStatoCivile.Coniugato), It.IsAny<Cliente>()), $"StatoCivile errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.Nome.Equals("nome1")), It.IsAny<Cliente>()), $"Nome errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.Cognome.Equals("cognome1")), It.IsAny<Cliente>()), $"Cognome errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.RagioneSociale.Equals("ragionesociale1")), It.IsAny<Cliente>()), $"RagioneSociale errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.Telefono.Equals("02234234233")), It.IsAny<Cliente>()), $"Telefono errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.Telefono2.Equals("0209877443")), It.IsAny<Cliente>()), $"Telefono2 errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.Cellulare.Equals("340123123123")), It.IsAny<Cliente>()), $"Cellulare errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.Fax.Equals("022348492389")), It.IsAny<Cliente>()), $"Fax errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.EMail.Equals("mario.rossi@mail.it")), It.IsAny<Cliente>()), $"EMail errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.EMail2.Equals("mario.rossi@mail2.it")), It.IsAny<Cliente>()), $"Email2 errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.SitoInternet.Equals("SitoInternet1")), It.IsAny<Cliente>()), $"SitoInternet errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.SkypeId.Equals("SkypeId1")), It.IsAny<Cliente>()), $"SkypeId errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.AltriRecapiti.Equals("AltriRecapiti1")), It.IsAny<Cliente>()), $"AltriRecapiti errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.TipoCanalePreferito == 6), It.IsAny<Cliente>()), $"TipoCanalePreferito errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.Note.Equals("Note1")), It.IsAny<Cliente>()), $"Note errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.TipoConsenso == 7), It.IsAny<Cliente>()), $"TipoConsenso errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.IdEditore == 8), It.IsAny<Cliente>()), $"Ideditore errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.TipoCliente == ETipoCliente.PersonaFisica), It.IsAny<Cliente>()), $"TipoCliente errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.TipoImpiego == ETipoImpiego.ImpPrivato), It.IsAny<Cliente>()), $"TipoImpiego errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.TipoSesso == ETipoSesso.Femmina), It.IsAny<Cliente>()), $"TipoSesso errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.TipoTitolo == ETipoTitolo.Avv), It.IsAny<Cliente>()), $"TipoTitolo errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.IdSettore == 13), It.IsAny<Cliente>()), $"Idsettore errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.AutoritaLuogoRilascio.Equals("AutoritaLuogoRilascio1")), It.IsAny<Cliente>()), $"AutoritaLuogoRilascio errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.CodiceCRM.Equals("CodiceCRM1")), It.IsAny<Cliente>()), $"CodiceCRM errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.CodiceFiscale.Equals("CodiceFiscale1")), It.IsAny<Cliente>()), $"CodiceFiscale errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.CodicePadre == 14), It.IsAny<Cliente>()), $"CodicePadre errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.DataRilascioDocumento.Equals(new DateTime(2019, 10, 15))), It.IsAny<Cliente>()), $"DataRilascioDocumento errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.DataScadenzaDocumento.Equals(new DateTime(2019, 11, 15))), It.IsAny<Cliente>()), $"DataScadenzaDocumento errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.NumeroDocumentoPresentato.Equals("NumeroDocumentoPresentato1")), It.IsAny<Cliente>()), $"NumeroDocumentoPresentato errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.Piva.Equals("Piva1")), It.IsAny<Cliente>()), $"Piva errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.TagCliente == 15), It.IsAny<Cliente>()), $"TagCliente errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.TipoDocumentoPresentato == ETipoDocumentoPresentato.Cartadiidentita), It.IsAny<Cliente>()), $"TipoDocumentoPresentato errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.Idorigine.Equals("Idorigine1")), It.IsAny<Cliente>()), $"Idorigine errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.NascitaLuogo.Equals("NascitaLuogo1")), It.IsAny<Cliente>()), $"NascitaLuogo errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.NascitaProvincia.Equals("NascitaProvincia1")), It.IsAny<Cliente>()), $"NascitaProvincia errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.NascitaData.Equals(new DateTime(2019, 12, 15))), It.IsAny<Cliente>()), $"NascitaData errato");
            mockMapper.Verify(m => m.Map(It.Is<AnagraficaSaveCommand>(x => x.EsenteAui == true), It.IsAny<Cliente>()), $"EsenteAui errato");
            mockContext.Verify();
            mockContext.Verify(x => x.SetRowVersionValue(It.IsAny<Cliente>(), It.Is<byte[]>(x => x.Length == 8 && x.SequenceEqual(new byte[8] { 90, 91, 92, 93, 99, 98, 92, 97 }))), Times.Once());
            mockContext.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task UpdateTest_TargetInvocationException_RecordNotFoundAsync()
        {
            Mock<AnagraficaContext> mockContext = new();
            Mock<IMapper> mockMapper = new();
            Mock<IAnagraficaCustomFilter> mockFilter = new();
            Mock<IAuthorizationManager> mockAuthorization = new();

            mockAuthorization.Setup(x => x.HasAuthorization(It.IsAny<EPrivilegioPoseidon>())).Returns(true);
            List<Cliente> clienti = new() { new Cliente() { Id = 1 }, new Cliente() { Id = 2 } };
            mockContext.SetupGet(c => c.Clienti).Returns(clienti.AsQueryable().BuildMockDbSet().Object);
            IAnagraficaService service = new AnagraficaService(mockContext.Object, null, mockMapper.Object, mockFilter.Object, mockAuthorization.Object);
            UpdatedModel response = await service.Update(3, new AnagraficaSaveCommand());
            Assert.IsNotNull(response);
            Assert.AreEqual(ApiErrorCode.NotFound, response.Result);
        }

        [TestMethod]
        public async Task UpdateTest_Wrong_First_Parameter()
        {
            Mock<IAuthorizationManager> mockAuthorization = new();

            mockAuthorization.Setup(x => x.HasAuthorization(It.IsAny<EPrivilegioPoseidon>())).Returns(true);
            IAnagraficaService service = new AnagraficaService(null, null, null, null, mockAuthorization.Object);
            _ = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await service.Update(0, new AnagraficaSaveCommand()));
        }

        [TestMethod]
        public async Task UpdateTest_Wrong_Second_Parameter()
        {
            Mock<IAuthorizationManager> mockAuthorization = new();

            mockAuthorization.Setup(x => x.HasAuthorization(It.IsAny<EPrivilegioPoseidon>())).Returns(true);
            IAnagraficaService service = new AnagraficaService(null, null, null, null, mockAuthorization.Object);
            _ = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await service.Update(1, null));
        }
    }
}
