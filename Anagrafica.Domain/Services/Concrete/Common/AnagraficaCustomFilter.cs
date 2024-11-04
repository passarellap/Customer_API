using AnagraficaAPI.Entity.Model;
using ServiceUtilities.Querying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TreePlat.Domain.Enumerations;

namespace AnagraficaAPI.Domain.Services
{
    public class AnagraficaCustomFilter : IAnagraficaCustomFilter
    {
        Expression<Func<Cliente, bool>> IAnagraficaCustomFilter.FilterId(int id) => x => x.Id == id;
        Expression<Func<Cliente, bool>> IAnagraficaCustomFilter.FilterIds(IEnumerable<int> ids) => x => ids.Contains(x.Id);
        Expression<Func<Cliente, bool>> IAnagraficaCustomFilter.FilterIdsTag(IEnumerable<int> idsTag)
        {
            Expression<Func<Cliente, bool>> exp = x => false;
            idsTag.ToList().ForEach(idtag => exp = exp.Or(x => x.RelClientiTags.Select(x => x.IdTag).Contains(idtag)));
            return exp;
        }

        Expression<Func<Cliente, bool>> IAnagraficaCustomFilter.FilterNominativo(string nominativo)
        {
            if (nominativo.Contains(" "))
            {
                string[] nomeCognome = nominativo.Split();

                Expression<Func<Cliente, bool>> exp = x => true;
                nomeCognome.ToList().ForEach(n => exp = exp.And(x => (x.TipoCliente == ETipoCliente.PersonaFisica && (x.Nome.Contains(n) || x.Cognome.Contains(n)) ) ||
                                                                     (x.TipoCliente == ETipoCliente.Societa && x.RagioneSociale.Contains(n))));
                return exp;
            }
            else
                return x => (x.TipoCliente == ETipoCliente.PersonaFisica && (x.Nome.Contains(nominativo) || x.Cognome.Contains(nominativo))) || 
                            (x.TipoCliente == ETipoCliente.Societa && x.RagioneSociale.Contains(nominativo));
        }

        Expression<Func<Cliente, bool>> IAnagraficaCustomFilter.FilterFreeText(string freeText)
        {
            if (freeText.Contains(" "))
            {
                string[] nomeCognome = freeText.Split();

                Expression<Func<Cliente, bool>> exp = x => true;
                nomeCognome.ToList().ForEach(n => exp = exp.And(x => (x.TipoCliente == ETipoCliente.PersonaFisica && (x.Nome.Contains(n) || x.Cognome.Contains(n))) ||
                                                                     (x.TipoCliente == ETipoCliente.Societa && x.RagioneSociale.Contains(n))));
                return exp;
            }
            else
                return x => (x.TipoCliente == ETipoCliente.PersonaFisica && (x.Nome.Contains(freeText) || x.Cognome.Contains(freeText))) || 
                            (x.TipoCliente == ETipoCliente.Societa && x.RagioneSociale.Contains(freeText)) || x.EMail.Contains(freeText) ||
                            x.Email2.Contains(freeText) || x.Telefono.Replace("/", "").Replace("-","").Replace(@"\", "").Replace(" ", "").Contains(freeText) || 
                            x.Telefono2.Replace("/", "").Replace("-", "").Replace(@"\", "").Replace(" ", "").Contains(freeText) ||
                            x.Cellulare.Replace("/", "").Replace("-", "").Replace(@"\", "").Replace(" ", "").Contains(freeText);
        }

        Expression<Func<Cliente, bool>> IAnagraficaCustomFilter.FilterTelefono(string telefono) 
            => x => x.Cellulare.Replace("/", "").Replace("-", "").Replace(@"\", "").Replace(" ", "").Contains(telefono) || 
                    x.Telefono.Replace("/", "").Replace("-", "").Replace(@"\", "").Replace(" ", "").Contains(telefono) ||
                    x.Telefono2.Replace("/", "").Replace("-", "").Replace(@"\", "").Replace(" ", "").Contains(telefono);

        Expression<Func<Cliente, bool>> IAnagraficaCustomFilter.FilterEmail(string email) => x => x.EMail.Contains(email) || x.Email2.Contains(email);
        Expression<Func<Cliente, bool>> IAnagraficaCustomFilter.FilterTipoCliente(ETipoCliente eTipoCliente) => x => x.TipoCliente == eTipoCliente;
        Expression<Func<Cliente, bool>> IAnagraficaCustomFilter.FilterStato(EStatoCliente eStatoCliente) => x => x.Stato == eStatoCliente && x.Stato != EStatoCliente.Eliminato;
        Expression<Func<Cliente, bool>> IAnagraficaCustomFilter.FilterStatoAttivo() => x => x.Stato == EStatoCliente.Attivo;
        Expression<Func<Cliente, bool>> IAnagraficaCustomFilter.FilterDataNascita(DateTime data) => x => x.NascitaData.HasValue && x.NascitaData.Value.Date >= data.Date;
        Expression<Func<Cliente, bool>> IAnagraficaCustomFilter.FilterPIva(string pIva) => x => x.Piva.Equals(pIva);
    }
}
