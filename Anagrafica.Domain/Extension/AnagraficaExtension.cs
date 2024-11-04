using AnagraficaAPI.Entity.Model;
using Microsoft.EntityFrameworkCore;
using ServiceUtilities.Querying;
using ServiceUtilities.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TreePlat.Domain.Enumerations;

namespace AnagraficaAPI.Domain.Extension
{
    public static class AnagraficaExtension
    {
        public static IEnumerable<string> AnagraficaOrderColumnsAvailables()
        {
            return new List<string>()
            {
                "Id",
                "Nominativo",
                "Nome",
                "Cognome",
                "RagioneSociale",
                "TipoCliente",
                "Stato",
                "NascitaData",
                "Created"
            };
        }

        public static bool AnagraficaValidateOrderBy(IEnumerable<string> orderByMultiple) =>
            !orderByMultiple
            .Select(x => x.RemoveLast(LamdaExpressionOrderBy.ORDERBY_DIRECTION_DESC_SUFFIX))
            .Where(p => !AnagraficaOrderColumnsAvailables()
            .Any(p2 => p2.ToLower() == p.ToLower())).Any();

        public static IOrderedQueryable<Cliente> AnagraficaOrderBy(this IQueryable<Cliente> source, string orderColumn, string defaultOrderBy)
        {
            if (orderColumn == null || orderColumn.Length == 0)
                return source.OrderByMultiple(new List<string> { defaultOrderBy }, defaultOrderBy);

            if (orderColumn.ToLower().Contains("cognome"))
            {
                return orderColumn.ToLower().Equals("cognome") ?
                       source.OrderBy(c => c.TipoCliente == ETipoCliente.PersonaFisica ? c.Cognome : c.RagioneSociale).ThenBy(c => c.Nome) :
                       source.OrderByDescending(c => c.TipoCliente == ETipoCliente.PersonaFisica ? c.Cognome : c.RagioneSociale).ThenByDescending(c => c.Nome);
            }
            else if (orderColumn.ToLower().Contains("nome"))
            {
                return orderColumn.ToLower().Equals("nome") ?
                       source.OrderBy(c => c.TipoCliente == ETipoCliente.PersonaFisica ? c.Nome : c.RagioneSociale).ThenBy(c => c.Cognome) :
                       source.OrderByDescending(c => c.TipoCliente == ETipoCliente.PersonaFisica ? c.Nome : c.RagioneSociale).ThenByDescending(c => c.Cognome);
            }
            else
            {
                return source.OrderByMultiple(new List<string> { orderColumn }, defaultOrderBy);
            }
        }

        public static IQueryable<Cliente> AnagraficaInclude(this IQueryable<Cliente> source)
        {
            return source
                .Include(x => x.Indirizzo)
                .Include(x => x.RelClientiTags)
                    .ThenInclude(x => x.Tag)
                .Include(x => x.DocumentoCliente)
                    .ThenInclude(x => x.Documento)
                ;
        }
    }
}
