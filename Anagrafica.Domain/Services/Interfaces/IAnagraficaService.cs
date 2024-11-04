using AnagraficaAPI.Domain.Command;
using AnagraficaAPI.Domain.Model;
using ServiceUtilities.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnagraficaAPI.Domain.Services
{
    public interface IAnagraficaService
    {
        Task<UpdatedModel> Delete(int id, byte[] rowVersion);
        Task<IEnumerable<AnagraficaModel>> Get(IEnumerable<int> ids);
        Task<IEnumerable<AnagraficaModel>> GetForAgenda(IEnumerable<int> ids);
        Task<AnagraficaModel> Get(int id);
        Task<UpdatedModel> Insert(AnagraficaSaveCommand data);
        Task<UpdatedModel> Update(int id, AnagraficaSaveCommand data);
        Task<PagedViewModel<AnagraficaModel>> GetListByFilter(AnagraficaFilterCommand request);
        Task<IEnumerable<string>> GetOrderColumnsAvailables();
        Task<int> AddTagToCliente(int id, int idtag);
        Task<int> RemoveTagFromCliente(int id, int idtag);
        Task<IEnumerable<AnagraficaModel>> GetListSortByFreeText(string nominativo);
    }
}