using AnagraficaAPI.Domain.Command;
using ServiceUtilities.Model;
using System.Threading.Tasks;

namespace AnagraficaAPI.Domain.Services
{
    public interface IIndirizzoService
    {
        Task<UpdatedModel> Insert(IndirizzoSaveCommand data);
        Task<UpdatedModel> Update(int id, IndirizzoSaveCommand data);
    }
}