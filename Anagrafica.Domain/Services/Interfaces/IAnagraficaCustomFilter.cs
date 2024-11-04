using AnagraficaAPI.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TreePlat.Domain.Enumerations;

namespace AnagraficaAPI.Domain.Services
{
    public interface IAnagraficaCustomFilter
    {
        Expression<Func<Cliente, bool>> FilterId(int id);
        Expression<Func<Cliente, bool>> FilterIds(IEnumerable<int> ids);
        Expression<Func<Cliente, bool>> FilterIdsTag(IEnumerable<int> idsTag);
        Expression<Func<Cliente, bool>> FilterNominativo(string nominativo);
        Expression<Func<Cliente, bool>> FilterFreeText(string freeText);
        Expression<Func<Cliente, bool>> FilterTelefono(string telefono);
        Expression<Func<Cliente, bool>> FilterEmail(string email);
        Expression<Func<Cliente, bool>> FilterTipoCliente(ETipoCliente eTipoCliente);
        Expression<Func<Cliente, bool>> FilterStato(EStatoCliente eStatoCliente);
        Expression<Func<Cliente, bool>> FilterStatoAttivo();
        Expression<Func<Cliente, bool>> FilterDataNascita(DateTime data);
        Expression<Func<Cliente, bool>> FilterPIva(string pIva);

    }
}