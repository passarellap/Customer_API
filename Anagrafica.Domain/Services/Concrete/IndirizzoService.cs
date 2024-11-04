using AnagraficaAPI.Domain.Command;
using AnagraficaAPI.Entity.Context;
using AnagraficaAPI.Entity.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ServiceUtilities.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using TreePlat.Domain.Constants;

namespace AnagraficaAPI.Domain.Services
{
    public class IndirizzoService : IIndirizzoService
    {
        private readonly AnagraficaContext _dbContext;
        private readonly IMapper _mapper;

        public IndirizzoService(AnagraficaContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UpdatedModel> Insert(IndirizzoSaveCommand data)
        {
            if (data != null)
            {
                EntityEntry<Indirizzo> indirizzoEntry = await _dbContext.AddAsync(_mapper.Map<Indirizzo>(data));
                int result = await _dbContext.SaveChangesAsync();
                Indirizzo indirizzo = indirizzoEntry?.Entity;
                if (indirizzo != null)
                    return new UpdatedModel(result, indirizzo.Id, indirizzo.RowVersion, indirizzo.Created, indirizzo.Modified.Value);
                else
                    return null;
            }
            else
                throw new ArgumentOutOfRangeException("Request", "Wrong parameters!");
        }

        public async Task<UpdatedModel> Update(int id, IndirizzoSaveCommand data)
        {
            if (id > 0 && data != null)
            {
                var indirizzo = await _dbContext.Indirizzi.Where(x => x.Id == id).SingleAsync();
                indirizzo = _mapper.Map(data, indirizzo);
                if (indirizzo != null)
                {
                    _dbContext.SetRowVersionValue(indirizzo, data.RowVersion);
                    int result = await _dbContext.SaveChangesAsync();
                    if (indirizzo != null)
                        return new UpdatedModel(result, indirizzo.Id, indirizzo.RowVersion, indirizzo.Created, indirizzo.Modified ?? DateTime.UtcNow);
                    else
                        return null;
                }
                else
                    return new UpdatedModel(ApiErrorCode.NotFound);
            }
            else
                throw new ArgumentOutOfRangeException("id/request", "Wrong parameters!");
        }
    }
}
