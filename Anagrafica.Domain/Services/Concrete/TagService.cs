using AnagraficaAPI.Domain.Command;
using AnagraficaAPI.Domain.Extension;
using AnagraficaAPI.Domain.Model;
using AnagraficaAPI.Entity.Context;
using AnagraficaAPI.Entity.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Poseidon.AuthorizationManager;
using ServiceUtilities.Model;
using ServiceUtilities.Paging;
using ServiceUtilities.Querying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreePlat.Domain.Constants;
using TreePlat.Domain.Enumerations;
using ZeusApi.AuthorizationException;

namespace AnagraficaAPI.Domain.Services
{
    public class TagService : ITagService
    {
        private readonly AnagraficaContext _dbContext;
        private readonly ILogger<TagService> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthorizationManager _authorizationManager;

        public TagService(AnagraficaContext dbContext, ILogger<TagService> logger, IMapper mapper, IAuthorizationManager authorizationManager)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _authorizationManager = authorizationManager;
        }

        public async Task<TagModel> GetTag(int id)
        {
            return _mapper.Map<TagModel>(await _dbContext.Tags.FindAsync(id));
        }

        public async Task<IEnumerable<TagModel>> GetListBestMatchTagName(string tagName)
        {
            IOrderedQueryable<Tag> query;

            if (string.IsNullOrEmpty(tagName?.Trim()))
                query = from tags in _dbContext.Tags orderby tags.TagName select tags;
            else
                query = from tags in _dbContext.Tags where tags.TagName.Contains(tagName) orderby _dbContext.LevenshteinMethod(tags.TagName, tagName, 100) select tags;

            return _mapper.Map<IEnumerable<Tag>, IEnumerable<TagModel>>(await query.ToListAsync());
        }

        public async Task<UpdatedModel> Insert(TagSaveCommand data)
        {
            if (_authorizationManager.HasAuthorization(EPrivilegioPoseidon.Cliente_ModificaTag))
            {
                if (data != null)
                {
                    int result = 1;
                    Tag tag = _dbContext.Tags.Where(x => x.TagName.ToLower().Equals(data.TagName.ToLower())).FirstOrDefault();
                    if (tag == null)
                    {
                        tag = _mapper.Map<Tag>(data);
                        if (tag != null)
                        {
                            tag.IdAgenzia = _authorizationManager.IdAgenzia;
                            tag.IdCollaboratore = _authorizationManager.IdCollaboratore;
                        }
                        EntityEntry<Tag> tagEntry = await _dbContext.AddAsync(tag);
                        result = await _dbContext.SaveChangesAsync();
                        tag = tagEntry?.Entity;
                    }
                    if (tag != null)
                        return new UpdatedModel(result, tag.Id, tag.RowVersion, tag.Created, tag.Modified.Value);
                    else
                        return null;
                }
                else
                    throw new ArgumentOutOfRangeException("Wrong parameters!");
            }
            else
                throw new ZeusApiAuthorizationException((int)EPrivilegioPoseidon.Cliente_ModificaTag);
        }

        public async Task<UpdatedModel> Update(int id, TagSaveCommand data)
        {
            if (_authorizationManager.HasAuthorization(EPrivilegioPoseidon.Cliente_ModificaTag))
            {
                if (id > 0 && data != null)
                {
                    Tag tag = _dbContext.Tags.Where(x => x.TagName.ToLower().Contains(data.TagName.ToLower())).FirstOrDefault();
                    if (tag == null)
                    {
                        tag = await _dbContext.Tags.FindAsync(id);
                        if (tag != null)
                        {
                            tag = _mapper.Map(data, tag);
                            _dbContext.SetRowVersionValue(tag, data.RowVersion);
                            int result = await _dbContext.SaveChangesAsync();
                            if (tag != null)
                                return new UpdatedModel(result, tag.Id, tag.RowVersion, tag.Created, tag.Modified ?? DateTime.UtcNow);
                            else
                                return null;
                        }
                        else
                            return new UpdatedModel(ApiErrorCode.NotFound);
                    }
                    else
                        throw new Exception("Already exist.");
                }
                else
                    throw new ArgumentOutOfRangeException("Wrong parameters!");
            }
            else
                throw new ZeusApiAuthorizationException((int)EPrivilegioPoseidon.Cliente_ModificaTag);
        }

        public async Task<int> DeleteTag(int id)
        {
            if (_authorizationManager.HasAuthorization(EPrivilegioPoseidon.Cliente_ModificaTag))
            {
                if (id > 0)
                {
                    if (_dbContext.RelClientiTags.Where(x => x.IdTag == id).Any() == false)
                    {
                        Tag tag = await _dbContext.Tags.FindAsync(id);
                        if (tag != null)
                        {
                            _dbContext.Tags.Remove(tag);
                            return await _dbContext.SaveChangesAsync();
                        }
                        else
                            return ApiErrorCode.NotFound;
                    }
                    else
                        throw new Exception("Il Tag è utilizzato da alcuni Clienti in anagrafica, per questo non è possibile eliminarlo.");
                }
                else
                    throw new ArgumentOutOfRangeException("Wrong parameters!");
            }
            else
                throw new ZeusApiAuthorizationException((int)EPrivilegioPoseidon.Cliente_ModificaTag);

        }

        public async Task<IEnumerable<TagModel>> GetRecentTags()
        {
            List<Tag> result = new();

            var queryRecent = _dbContext.RelClientiTags
                .Include(x => x.Tag)
                .GroupBy(clt => clt.IdTag)
                .Select(group => new { id = group.Key, Count = group.Count(), LastDate = group.Max(r => r.Created) })
                .OrderByDescending(v => v.Count)
                .ThenByDescending(v => v.LastDate);

            var alltags = await _dbContext.Tags.ToListAsync();
            (await queryRecent.ToListAsync()).Select(y => y.id).ToList().ForEach(y => result.Add(alltags.Where(x => x.Id == y).FirstOrDefault()));

            return _mapper.Map<IEnumerable<TagModel>>(result);
        }

        public async Task<PagedViewModel<TagModel>> GetPagedListBestMatchTagName(string tagName)
        {
            IOrderedQueryable<Tag> query;

            if (string.IsNullOrEmpty(tagName?.Trim()))
                query = from tags in _dbContext.Tags orderby tags.TagName select tags;
            else
                query = from tags in _dbContext.Tags where tags.TagName.Contains(tagName) orderby _dbContext.LevenshteinMethod(tags.TagName, tagName, 100) select tags;

            var sql = query.ToQueryString();

            PagedList<Tag> pagedList = await PagedList<Tag>.ToPagedListAsync(query, TagExtension.TagOrderColumnsAvailables(), 0, 20, false);
            _logger.LogDebug($"Tempo esecuzione RowsCount() : {pagedList.TimeExecutionRowsCount}");
            _logger.LogDebug($"Tempo esecuzione Query() : {pagedList.TimeExecutionQuery}");
            _logger.LogDebug($"SQL Query() : {pagedList.SqlQuery}");
            return _mapper.Map<PagedList<Tag>, PagedViewModel<TagModel>>(pagedList);
        }
    }
}

