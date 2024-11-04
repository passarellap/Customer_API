using AnagraficaAPI.Domain.Model;
using AnagraficaAPI.Domain.Services;
using AnagraficaAPI.Entity.Context;
using AnagraficaAPI.Entity.Model;
using AutoMapper;
using DocsGeneratorService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using Poseidon.AuthorizationManager;
using ServiceUtilities.Model;
using ServiceUtilities.Paging;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest.AnagraficaAPI
{
    public static class UnitTestUtils
    {
        public static void VerifyRecord(this Mock<IMapper> mockMapper, int id)
        {
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Where(x => x.Id == id).Any())), $"Record id={id} non presente");
        }

        public static void VerifyCount(this Mock<IMapper> mockMapper, int count)
        {
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.Count == count)), $"n. Records errati, attesi {count}");
        }

        public static void VerifyListRecord(this Mock<IMapper> mockMapper, int id)
        {
            mockMapper.Verify(m => m.Map<IEnumerable<AnagraficaModel>>(It.Is<List<Cliente>>(l => l.Where(x => x.Id == id).Any())), $"Record id={id} non presente");
        }

        public static void VerifyListCount(this Mock<IMapper> mockMapper, int count)
        {
            mockMapper.Verify(m => m.Map<IEnumerable<AnagraficaModel>>(It.Is<List<Cliente>>(l => l.Count == count)), $"n. Records errati, attesi {count}");
        }

        public static void VerifyRecordOrder(this Mock<IMapper> mockMapper, int id, int position)
        {
            mockMapper.Verify(m => m.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(It.Is<PagedList<Cliente>>(l => l.ElementAt(position).Id == id)), $"Record id={id} non presente");
        }

        public static void VerifyListRecordOrder(this Mock<IMapper> mockMapper, int id, int position)
        {
            mockMapper.Verify(m => m.Map<IEnumerable<AnagraficaModel>>(It.Is<List<Cliente>>(l => l.ElementAt(position).Id == id)), $"Record id={id} non presente");
        }
    }
}
