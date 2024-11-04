using AnagraficaAPI.Domain.Command;
using System.Threading.Tasks;

namespace AnagraficaAPI.Domain.Services
{
    public interface IExportService
    {
        Task<byte[]> GetExcelEstrazioneClienti(AnagraficaFilterCommand command);
    }
}