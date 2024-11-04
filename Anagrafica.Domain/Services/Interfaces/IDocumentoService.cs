using AnagraficaAPI.Domain.Command;
using AnagraficaAPI.Domain.Model;
using System.Threading.Tasks;

namespace AnagraficaAPI.Domain.Services
{
    public interface IDocumentoService
    {
        Task<int?> Insert(DocumentoSaveCommand data);
        Task<int?> Update(int id, DocumentoSaveCommand data);
        Task<int> Remove(int idDocumento);
        Task<DocumentoModel> GetDocumento(int id);
    }
}