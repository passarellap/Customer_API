using AnagraficaAPI.Domain.Command;
using AnagraficaAPI.Domain.Model;
using ServiceUtilities.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnagraficaAPI.Domain.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagModel>> GetListBestMatchTagName(string tagName);
        Task<TagModel> GetTag(int id);
        Task<UpdatedModel> Insert(TagSaveCommand data);
        Task<UpdatedModel> Update(int id, TagSaveCommand data);
        Task<int> DeleteTag(int id);
        Task<IEnumerable<TagModel>> GetRecentTags();
        Task<PagedViewModel<TagModel>> GetPagedListBestMatchTagName(string tagName);
    }
}