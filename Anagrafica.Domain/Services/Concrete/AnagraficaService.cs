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
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TreePlat.Domain.Constants;
using TreePlat.Domain.Enumerations;
using TreePlat.Domain.Enumerations.Base;
using ZeusApi.AuthorizationException;

namespace AnagraficaAPI.Domain.Services
{
    public class AnagraficaService : IAnagraficaService
    {
        private readonly AnagraficaContext _dbContext;
        private readonly ILogger<AnagraficaService> _logger;
        private readonly IMapper _mapper;
        private readonly IAnagraficaCustomFilter _customFilter;
        private readonly IAuthorizationManager _authorizationManager;

        public AnagraficaService(AnagraficaContext dbContext, ILogger<AnagraficaService> logger, IMapper mapper, IAnagraficaCustomFilter customFilter, IAuthorizationManager authorizationManager)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _authorizationManager = authorizationManager;
            _customFilter = customFilter;
        }

        public async Task<AnagraficaModel> Get(int id) =>
            _mapper.Map<AnagraficaModel>(await _dbContext.Clienti
                .AnagraficaInclude()
                .Where(_customFilter.FilterId(id))
                .AsNoTracking()
                .AsSplitQuery()
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync());

        public async Task<IEnumerable<AnagraficaModel>> Get(IEnumerable<int> ids)
        {
            List<Cliente> clientiList = new();

            if (ids.Any())
                clientiList = await _dbContext.Clienti
                    .AnagraficaInclude()
                    .Where(_customFilter.FilterIds(ids))
                    .AsNoTracking()
                    .AsSplitQuery()
                    .OrderBy(x => x.Id)
                    .ToListAsync();

            return _mapper.Map<IEnumerable<AnagraficaModel>>(clientiList);
        }

        public async Task<IEnumerable<AnagraficaModel>> GetForAgenda(IEnumerable<int> ids)
        {
            List<Cliente> clientiList = new();

            if (ids.Any())
                clientiList = await _dbContext.Clienti
                    .Where(_customFilter.FilterIds(ids))
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .ToListAsync();

            return _mapper.Map<IEnumerable<AnagraficaModel>>(clientiList);
        }

        public async Task<PagedViewModel<AnagraficaModel>> GetListByFilter(AnagraficaFilterCommand request)
        {
            if (AnagraficaExtension.AnagraficaValidateOrderBy(request.OrderBy))
            {
                PagedList<Cliente> pagedList = await PagedList<Cliente>.ToPagedListAsync(GetListQuery(request), AnagraficaExtension.AnagraficaOrderColumnsAvailables(), request.PageNumber, request.PageSize, false);
                _logger.LogDebug($"Tempo esecuzione RowsCount() : {pagedList.TimeExecutionRowsCount}");
                _logger.LogDebug($"Tempo esecuzione Query() : {pagedList.TimeExecutionQuery}");
                _logger.LogDebug($"Query : {pagedList.SqlQuery}");
                return _mapper.Map<PagedList<Cliente>, PagedViewModel<AnagraficaModel>>(pagedList);
            }
            else
                throw new Exception("Invalid OrderBy Columns");
        }

        public async Task<UpdatedModel> Insert(AnagraficaSaveCommand data)
        {
            _logger.LogDebug($"Started method Insert()");
            CheckPrivilegio(EPrivilegioPoseidon.Cliente_Modifica);
            if (data != null)
            {
                Cliente cliente = _mapper.Map<Cliente>(data);
                if (cliente != null)
                {
                    cliente.Stato = EStatoCliente.Attivo;
                    cliente.IdAgenzia = _authorizationManager.IdAgenzia;
                    cliente.IdCollaboratore = _authorizationManager.IdCollaboratore;
                }
                EntityEntry<Cliente> clienteEntry = await _dbContext.AddAsync(cliente);
                int result = await _dbContext.SaveChangesAsync();
                cliente = clienteEntry?.Entity;
                if (cliente != null)
                    return new UpdatedModel(result, cliente.Id, cliente.RowVersion, cliente.Created, cliente.Modified.Value);
                else
                    return null;
            }
            else
                throw new ArgumentOutOfRangeException(nameof(data), "Wrong parameters!");
        }

        public async Task<UpdatedModel> Update(int id, AnagraficaSaveCommand data)
        {
            CheckPrivilegio(EPrivilegioPoseidon.Cliente_Modifica);
            if (id > 0 && data != null)
            {
                Cliente cliente = await _dbContext.Clienti.FindAsync(id);
                if (cliente != null && cliente.Stato != EStatoCliente.Eliminato)
                {
                    cliente = _mapper.Map(data, cliente);
                    _dbContext.SetRowVersionValue(cliente, data.RowVersion);
                    int result = await _dbContext.SaveChangesAsync();
                    if (cliente != null)
                        return new UpdatedModel(result, cliente.Id, cliente.RowVersion, cliente.Created, cliente.Modified ?? DateTime.UtcNow);
                    else
                        return null;
                }
                else
                    return new UpdatedModel(ApiErrorCode.NotFound);
            }
            else
                throw new ArgumentOutOfRangeException(nameof(data), "Wrong parameters!");
        }

        public async Task<UpdatedModel> Delete(int id, byte[] rowVersion)
        {
            CheckPrivilegio(EPrivilegioPoseidon.Cliente_Eliminazione);
            Cliente cliente = await _dbContext.Clienti.Include(x => x.Indirizzo).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (cliente != null && cliente.Stato != EStatoCliente.Eliminato)
            {
                cliente.Stato = EStatoCliente.Eliminato;
                cliente.Nome = " ";
                cliente.Cognome = "Nominativo Cancellato";
                cliente.Telefono = "";
                cliente.Telefono2 = "";
                cliente.SkypeId = "";
                cliente.RagioneSociale = "Ragione Sociale Cancellata";
                cliente.AltriRecapiti = "";
                cliente.Cellulare = "";
                cliente.EMail = "";
                cliente.Email2 = "";
                cliente.Fax = "";
                cliente.SitoInternet = "";
                cliente.StatoCivile = EStatoCivile.NonSpecificato;
                cliente.TipoCanalePreferito = ETipoCanalePreferito.NonSpecificato;
                cliente.TipoImpiego = ETipoImpiego.NonSpecificato;
                cliente.TipoSesso = ETipoSesso.NonSpecificato;
                cliente.TipoTitolo = ETipoTitolo.NonSpecificato;
                cliente.CodiceFiscale = "";
                cliente.Piva = "";
                cliente.NumeroDocumentoPresentato = "";
                cliente.TipoDocumentoPresentato = ETipoDocumentoPresentato.NonSpecificato;
                cliente.AutoritaLuogoRilascio = "";
                cliente.DataRilascioDocumento = ValoriDefault.DATA_NON_INSERITA;
                cliente.DataScadenzaDocumento = ValoriDefault.DATA_NON_INSERITA;
                cliente.NascitaData = ValoriDefault.DATA_NON_INSERITA;
                cliente.NascitaLuogo = "";
                cliente.NascitaProvincia = "";
                cliente.Note = "";

                if (cliente.Indirizzo != null)
                {
                    _dbContext.Remove(cliente.Indirizzo);
                    cliente.Indirizzo = null;
                }

                _dbContext.SetRowVersionValue(cliente, rowVersion);
                int result = await _dbContext.SaveChangesAsync();
                return new UpdatedModel(result, cliente.Id, cliente.RowVersion, cliente.Created, cliente.Modified.Value);
            }
            else
                return new UpdatedModel() { Result = ApiErrorCode.NotFound };
        }

        public async Task<int> AddTagToCliente(int id, int idtag)
        {
            CheckPrivilegio(EPrivilegioPoseidon.Cliente_ModificaTag);

            if (id > 0 && idtag > 0)
            {
                Cliente cliente = await _dbContext.Clienti.FindAsync(id);
                Tag tag = await _dbContext.Tags.FindAsync(idtag);

                if (cliente != null && cliente.Stato != EStatoCliente.Eliminato && tag != null)
                {
                    RelClientiTags relClienteTag = await _dbContext.RelClientiTags.FindAsync(id, idtag);
                    if (relClienteTag == null)
                    {
                        relClienteTag = new();
                        relClienteTag.IdCliente = id;
                        relClienteTag.IdTag = idtag;
                        relClienteTag.Created = DateTime.UtcNow;
                        await _dbContext.AddAsync(relClienteTag);
                        return await _dbContext.SaveChangesAsync();
                    }
                    else
                        throw new Exception("Tag già presente nel cliente.");
                }
                else
                    return ApiErrorCode.NotFound;
            }
            else
                throw new ArgumentOutOfRangeException("IdRichiesta o IdTag", "Wrong parameters!");
        }

        public async Task<int> RemoveTagFromCliente(int id, int idtag)
        {
            CheckPrivilegio(EPrivilegioPoseidon.Cliente_ModificaTag);

            if (id > 0 && idtag > 0)
            {
                RelClientiTags relClientiTags = await _dbContext.RelClientiTags.FindAsync(id, idtag);
                if (relClientiTags != null)
                {
                    _dbContext.RelClientiTags.Remove(relClientiTags);
                    return await _dbContext.SaveChangesAsync();
                }
                else
                    return ApiErrorCode.NotFound;
            }
            else
                throw new ArgumentOutOfRangeException("IdRichiesta o IdTag", "Wrong parameters!");
        }

        public async Task<IEnumerable<string>> GetOrderColumnsAvailables()
        {
            await Task.Delay(1);
            return AnagraficaExtension.AnagraficaOrderColumnsAvailables();
        }

        public async Task<IEnumerable<AnagraficaModel>> GetListSortByFreeText(string freeText)
        {
            if (string.IsNullOrEmpty(freeText))
                throw new ArgumentOutOfRangeException(nameof(freeText), "Wrong parameters!");

            List<Cliente> clienti = await _dbContext.Clienti.Where(_customFilter.FilterFreeText(freeText))
                                                            .Where(x => x.Stato == EStatoCliente.Attivo)
                                                            .ToListAsync();
            if (freeText.Contains(' '))
            {
                string[] nomeCognome = freeText.Split();
                string nome = nomeCognome[0];
                string cognome = nomeCognome[1];
                clienti = clienti.OrderByDescending(c => StringSimilarityScore(c, nome.ToLower(), cognome.ToLower(), freeText.ToLower())).ToList();
            }
            else
                clienti = clienti.OrderByDescending(c => StringSimilarityScore(c, null, null, freeText.ToLower())).ToList();

            return _mapper.Map<IEnumerable<AnagraficaModel>>(clienti);
        }

        private static int StringSimilarityScore(Cliente cliente, string nome, string cognome, string nominativo)
        {
            string nomeCliente = cliente.Nome.ToLower();
            string cognomeCliente = cliente.Cognome.ToLower();
            string ragSoc = cliente.RagioneSociale.ToLower();
            ETipoCliente tipoCliente = cliente.TipoCliente;

            if (nome != null && cognome != null)
            {
                if (tipoCliente == ETipoCliente.PersonaFisica)
                {
                    string nomecognome = $"{nomeCliente} {cognomeCliente}";
                    string cognomenome = $"{cognomeCliente} {nomeCliente}";

                    if (nomecognome.Equals(nominativo) || cognomenome.Equals(nominativo))
                        return 9;
                    else if (nomecognome.StartsWith(nominativo) || cognomenome.StartsWith(nominativo))
                        return 8;
                    else if (nomecognome.Contains(nominativo) || cognomenome.Contains(nominativo))
                        return 7;
                    else if ((nomeCliente.Equals(nome) && cognomeCliente.Equals(cognome)) || (nomeCliente.Equals(cognome) && cognomeCliente.Equals(nome)) || (nomeCliente.Equals(nominativo) || cognomeCliente.Equals(nominativo)))
                        return 6;
                    else if ((nomeCliente.Equals(nome) && cognomeCliente.StartsWith(cognome)) || (nomeCliente.StartsWith(cognome) && cognomeCliente.Equals(nome)) || (nomeCliente.StartsWith(nominativo) || cognomeCliente.StartsWith(nominativo)))
                        return 5;
                    else if ((nomeCliente.Equals(nome) && cognomeCliente.Contains(cognome)) || (nomeCliente.Contains(cognome) && cognomeCliente.Equals(nome)) || (nomeCliente.Contains(nominativo) || cognomeCliente.Contains(nominativo)))
                        return 4;
                    else if ((nomeCliente.StartsWith(nome) && cognomeCliente.StartsWith(cognome)) || (nomeCliente.StartsWith(cognome) && cognomeCliente.StartsWith(nome)))
                        return 3;
                    else if ((nomeCliente.Contains(nome) && cognomeCliente.StartsWith(cognome)) || (nomeCliente.StartsWith(cognome) && cognomeCliente.Contains(nome)))
                        return 2;
                    else if ((nomeCliente.Contains(nome) && cognomeCliente.Contains(cognome)) || (nomeCliente.Contains(cognome) && cognomeCliente.Equals(nome)))
                        return 1;
                    else
                        return 0;
                }
                else
                {
                    if (ragSoc.Equals(nominativo))
                        return 9;
                    else if (ragSoc.StartsWith(nominativo))
                        return 8;
                    else if (ragSoc.Contains(nominativo))
                        return 1;
                    else
                        return 0;
                }
            }
            else
            {
                string telefono = FormatPhoneNumber(cliente.Telefono);
                string telefono2 = FormatPhoneNumber(cliente.Telefono2);
                string cellulare = FormatPhoneNumber(cliente.Cellulare);
                string email = cliente.EMail;
                string email2 = cliente.Email2;

                if (tipoCliente == ETipoCliente.PersonaFisica)
                {
                    if (nomeCliente.Equals(nominativo) || cognomeCliente.Equals(nominativo) || telefono.Equals(nominativo) || telefono2.Equals(nominativo) || cellulare.Equals(nominativo) || email.Equals(nominativo) || email2.Equals(nominativo))
                        return 6;
                    else if (nomeCliente.StartsWith(nominativo) || cognomeCliente.StartsWith(nominativo) || telefono.StartsWith(nominativo) || telefono2.StartsWith(nominativo) || cellulare.StartsWith(nominativo) || email.StartsWith(nominativo) || email2.StartsWith(nominativo))
                        return 5;
                    else if (nomeCliente.Contains(nominativo) || cognomeCliente.Contains(nominativo) || telefono.Contains(nominativo) || telefono2.Contains(nominativo) || cellulare.Contains(nominativo) || email.Contains(nominativo) || email2.Contains(nominativo))
                        return 4;
                    else
                        return 0;
                }
                else
                {
                    if (ragSoc.Equals(nominativo) || telefono.Equals(nominativo) || telefono2.Equals(nominativo) || cellulare.Equals(nominativo) || email.Equals(nominativo) || email2.Equals(nominativo))
                        return 6;
                    else if (ragSoc.StartsWith(nominativo) || telefono.StartsWith(nominativo) || telefono2.StartsWith(nominativo) || cellulare.StartsWith(nominativo) || email.StartsWith(nominativo) || email2.StartsWith(nominativo))
                        return 5;
                    else if (ragSoc.Contains(nominativo) || telefono.Contains(nominativo) || telefono2.Contains(nominativo) || cellulare.Contains(nominativo) || email.Contains(nominativo) || email2.Contains(nominativo))
                        return 4;
                    else
                        return 0;
                }
            }
        }

        private static string FormatPhoneNumber(string tel) => Regex.Replace(tel, "[- /]", "").Replace(@"\", "");

        private IOrderedQueryable<Cliente> GetListQuery(AnagraficaFilterCommand request)
        {
            Expression<Func<Cliente, bool>> queryEspression = x => true;

            if (request.Id.HasValue) queryEspression = queryEspression.And(_customFilter.FilterId(request.Id.Value));
            if (request.Ids != null && request.Ids.Any()) queryEspression = queryEspression.And(_customFilter.FilterIds(request.Ids));
            if (request.IdsTag != null && request.IdsTag.Any()) queryEspression = queryEspression.And(_customFilter.FilterIdsTag(request.IdsTag));
            if (!string.IsNullOrEmpty(request.Nominativo)) queryEspression = queryEspression.And(_customFilter.FilterNominativo(request.Nominativo));
            if (!string.IsNullOrEmpty(request.Telefono)) queryEspression = queryEspression.And(_customFilter.FilterTelefono(request.Telefono));
            if (!string.IsNullOrEmpty(request.EMail)) queryEspression = queryEspression.And(_customFilter.FilterEmail(request.EMail));
            if (request.TipoCliente != null && EnumUtil.TryParseEnum(request.TipoCliente.ToString(), out ETipoCliente? eTipoCliente))
                queryEspression = queryEspression.And(_customFilter.FilterTipoCliente(eTipoCliente.Value));
            if (request.Stato != null && EnumUtil.TryParseEnum(request.Stato.ToString(), out EStatoCliente? eStatoCliente))
                queryEspression = queryEspression.And(_customFilter.FilterStato(eStatoCliente.Value));
            else
                queryEspression = queryEspression.And(_customFilter.FilterStatoAttivo());
            if (request.NascitaData.HasValue) queryEspression = queryEspression.And(_customFilter.FilterDataNascita(request.NascitaData.Value));
            if (!string.IsNullOrEmpty(request.FreeText)) queryEspression = queryEspression.And(_customFilter.FilterFreeText(request.FreeText));
            if (!string.IsNullOrEmpty(request.PIva)) queryEspression = queryEspression.And(_customFilter.FilterPIva(request.PIva));

            IOrderedQueryable<Cliente> query = _dbContext.Clienti
                .AnagraficaInclude()
                .Where(queryEspression)
                .AsNoTracking()
                .AsSplitQuery()
                .AnagraficaOrderBy(request.OrderBy.FirstOrDefault(), "Id");


            var sql = query.ToQueryString();
            return query;
        }

        private void CheckPrivilegio(EPrivilegioPoseidon privilegio)
        {
            if (!_authorizationManager.HasAuthorization(privilegio))
                throw new ZeusApiAuthorizationException((int)privilegio);
        }
    }
}

