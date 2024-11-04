using AnagraficaAPI.Domain.Command;
using DocsGeneratorService;
using Microsoft.Extensions.Logging;
using Poseidon.AuthorizationManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreePlat.Domain.Enumerations;
using ZeusApi.AuthorizationException;

namespace AnagraficaAPI.Domain.Services
{
    public class ExportService : IExportService
    {
        private readonly ILogger<ExportService> _logger;
        private readonly IAuthorizationManager _authorizationManager;
        private readonly IDocService _docClient;

        public ExportService(ILogger<ExportService> logger, IAuthorizationManager authorizationManager, IDocService docClient)
        {
            _logger = logger;
            _authorizationManager = authorizationManager;
            _docClient = docClient;
        }

        public async Task<byte[]> GetExcelEstrazioneClienti(AnagraficaFilterCommand command)
        {
            GetExcelFromQueryRequest request = GetExcelRequest(command);

            try
            {
                GetExcelFromQueryResponse result = await _docClient.GetExcelEsportazioneClientiFromQueryAsync(request);

                if (result.Data == null)
                {
                    _logger.LogDebug("Error calling GetExcelEsportazioneClientiFromQuery, ResCode: " + result.ResCode + ", Message: " + result.ResMessage);
                }
                return result.Data;
            }
            catch (Exception e)
            {
                _logger.LogDebug("Catched Exception calling GetExcelEsportazioneClientiFromQuery of DocsGenerator, error message: " + e.Message);
                throw;
            }
        }

        public GetExcelFromQueryRequest GetExcelRequest(AnagraficaFilterCommand command)
        {
            if (!_authorizationManager.HasAuthorization(EPrivilegioPoseidon.Cliente_Esportazione))
                throw new ZeusApiAuthorizationException((int)EPrivilegioPoseidon.Cliente_Esportazione);

            List<FilterDTO> filterList = new();

            if (command.Nominativo != null && command.Nominativo.Length > 0) filterList.Add(GetFilter("Nominativo", EFilterOperatorType.EqualTo, command.Nominativo, EFilterValueType.String, null));
            if (command.Telefono != null && command.Telefono.Length > 0) filterList.Add(GetFilter("Telefono", EFilterOperatorType.EqualTo, command.Telefono, EFilterValueType.String, null));
            if (command.EMail != null && command.EMail.Length > 0) filterList.Add(GetFilter("EMail", EFilterOperatorType.EqualTo, command.EMail, EFilterValueType.String, null));
            if (command.TipoCliente.HasValue) filterList.Add(GetFilter("TipoCliente", EFilterOperatorType.EqualTo, command.TipoCliente.Value, EFilterValueType.NumericInt, null));
            if (command.NascitaData.HasValue)
            {
                DateTime dateTime = new(command.NascitaData.Value.Year, command.NascitaData.Value.Month, command.NascitaData.Value.Day);
                filterList.Add(GetFilter("NascitaData", EFilterOperatorType.GreaterThanOrEqualTo, dateTime.ToString("yyyy/MM/dd HH:mm:ss"), EFilterValueType.Date, null));
            }

            if (command.IdsTag != null && command.IdsTag.Any())
            {
                if (command.IdsTag.Count() == 1)
                {
                    filterList.Add(GetFilter("IDTag", EFilterOperatorType.EqualTo, (int)command.IdsTag.ElementAt(0), EFilterValueType.NumericInt, null));
                }
                else
                {
                    List<SubFilterDTO> subFilterList = new();

                    for (int i = 1; i < command.IdsTag.Count(); i++)
                        subFilterList.Add(GetSubFilter("IDTag", EFilterOperatorType.EqualTo, (int)command.IdsTag.ElementAt(i), EFilterValueType.NumericInt));

                    filterList.Add(GetFilter("IDTag", EFilterOperatorType.EqualTo, (int)command.IdsTag.ElementAt(0), EFilterValueType.NumericInt, subFilterList));
                }
            }
            
            filterList.Add(GetFilter("Stato", EFilterOperatorType.EqualTo, (int)EStatoCliente.Attivo, EFilterValueType.NumericInt, null));

            filterList.Add(GetFilter("IDGestione", EFilterOperatorType.EqualTo, _authorizationManager.IdAgenzia, EFilterValueType.NumericInt, null));

            GetExcelFromQueryRequest request = new()
            {
                ViewName = "[dbo].[Viw_ClientiEsportazione]",
                TemplateFileName = "Estrazione_Clienti.xltx",
                QueryObject = new QueryObjectDTO()
                {
                    FilterList = filterList.ToArray()
                }
            };

            return request;
        }

        private static FilterDTO GetFilter(string field, EFilterOperatorType filteroperator, object value, EFilterValueType valueType, List<SubFilterDTO> subFilterList)
            => new()
            {
                Field = field,
                FilterOperator = (int)filteroperator,
                Value = value.ToString(),
                ValueType = (int)valueType,
                SubFilterList = subFilterList?.ToArray()
            };


        private static SubFilterDTO GetSubFilter(string field, EFilterOperatorType filteroperator, object value, EFilterValueType valueType)
            => new()
            {
                Field = field,
                FilterOperator = (int)filteroperator,
                Value = value.ToString(),
                ValueType = (int)valueType
            };
    }
}
