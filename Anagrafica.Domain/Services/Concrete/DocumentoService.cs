using AnagraficaAPI.Domain.Command;
using AnagraficaAPI.Domain.Model;
using AnagraficaAPI.Entity.Context;
using AnagraficaAPI.Entity.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Poseidon.AuthorizationManager;
using System;
using System.Linq;
using System.Threading.Tasks;
using TreePlat.Domain.Constants;
using TreePlat.Domain.Enumerations;
using ZeusApi.AuthorizationException;

namespace AnagraficaAPI.Domain.Services
{
    public class DocumentoService : IDocumentoService
    {
        private readonly AnagraficaContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAuthorizationManager _authorizationManager;

        public DocumentoService(AnagraficaContext dbContext, IMapper mapper, IAuthorizationManager authorizationManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _authorizationManager = authorizationManager;
        }

        public async Task<int?> Insert(DocumentoSaveCommand data)
        {
            if (data != null)
            {
                var cliente = await _dbContext.Clienti.FindAsync(data.IdCliente);
                if (cliente == null)
                    return ApiErrorCode.NotFound;
                CheckPrivilegi();

                DocumentoCliente documentoCliente = new()
                {
                    Documento = new Documento
                    {
                        Descrizione = data.Descrizione,
                        NomeFile = data.Nomefile,
                        Estensione = data.Estensione,
                        TipoEntita = ETipoEntitaDocumento.Cliente,
                        Dimensione = data.Dimensione,
                        NomeFileUtente = data.NomeFileUtente,
                        Url = data.Url,
                        Created = DateTime.UtcNow
                    },
                    IdCliente = data.IdCliente,
                };

                EntityEntry<DocumentoCliente> documentoClienteEntry = await _dbContext.AddAsync(documentoCliente);
                int result = await _dbContext.SaveChangesAsync();
                if (result > 0 && documentoClienteEntry != null)
                    return documentoClienteEntry.Entity.IdDocumento;
                else
                    return null;
            }
            else
                throw new ArgumentOutOfRangeException("Wrong parameters!");
        }


        public async Task<int?> Update(int id, DocumentoSaveCommand data)
        {
            if (id > 0 && data != null)
            {
                var cliente = await _dbContext.Clienti.FindAsync(data.IdCliente);
                if (cliente == null)
                    return ApiErrorCode.NotFound;
                CheckPrivilegi();

                Documento documento = await _dbContext.Documenti.FindAsync(id);
                DocumentoCliente documentoCliente = await _dbContext.DocumentoCliente.FindAsync(id);
                if (documentoCliente != null && documento != null)
                {
                    documento.Descrizione = data.Descrizione;
                    documento.NomeFile = data.Nomefile;
                    documento.Estensione = data.Estensione;
                    documento.NomeFileUtente = data.NomeFileUtente;
                    documento.Url = data.Url;
                    documento.Dimensione = data.Dimensione;
                    return await _dbContext.SaveChangesAsync();
                }
                else
                    return ApiErrorCode.NotFound;
            }
            else
                throw new ArgumentOutOfRangeException("Wrong parameters!");
        }

        public async Task<int> Remove(int idDocumento)
        {
            Documento documento = await _dbContext.Documenti.FindAsync(idDocumento);
            DocumentoCliente documentoCliente = await _dbContext.DocumentoCliente.FindAsync(idDocumento);
            if (documentoCliente != null && documento != null)
            {
                Cliente cliente = await _dbContext.Clienti.FindAsync(documentoCliente.IdCliente);
                if (cliente == null)
                    return ApiErrorCode.NotFound;
                CheckPrivilegi();

                _dbContext.DocumentoCliente.Remove(documentoCliente);
                _dbContext.Documenti.Remove(documento);
                return await _dbContext.SaveChangesAsync();
            }
            else
                return ApiErrorCode.NotFound;
        }

        public async Task<DocumentoModel> GetDocumento(int id)
        {
            DocumentoCliente documentoCliente = await _dbContext.DocumentoCliente.Include(x => x.Documento).Where(x => x.IdDocumento == id).FirstOrDefaultAsync();
            if (documentoCliente != null)
            {
                Cliente cliente = await _dbContext.Clienti.FindAsync(documentoCliente.IdCliente);
                if (cliente == null)
                    return null;

                return _mapper.Map<DocumentoModel>(documentoCliente);
            }
            else
                return null;
        }

        private void CheckPrivilegi()
        {
            if (!_authorizationManager.HasAuthorization(EPrivilegioPoseidon.Cliente_Modifica))
                throw new ZeusApiAuthorizationException((int)EPrivilegioPoseidon.Cliente_Modifica);
        }
    }
}
