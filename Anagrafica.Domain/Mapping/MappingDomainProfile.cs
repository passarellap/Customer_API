using AnagraficaAPI.Domain.Command;
using AnagraficaAPI.Domain.Model;
using AnagraficaAPI.Entity.Model;
using AutoMapper;
using ServiceUtilities.Model;
using ServiceUtilities.Paging;
using System;
using System.Linq;
using TreePlat.Domain.Enumerations;

namespace AnagraficaAPI.Domain.Mapping
{
    public class MappingDomainProfile : Profile
    {
        public MappingDomainProfile()
        {
            CreateMap<Tag, TagModel>();
            CreateMap<Indirizzo, IndirizzoModel>();
            CreateMap<IndirizzoSaveCommand, Indirizzo>();
            CreateMap<Cliente, AnagraficaModel>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.RelClientiTags.Select(x => x.Tag)))
                .ForMember(dest => dest.TipoCanalePreferito, o => o.MapFrom(src => GetTipoCanale(src.TipoCanalePreferito)))
                .ForMember(dest => dest.StatoCivile, o => o.MapFrom(src => GetStatoCivile(src.StatoCivile)))
                .ForMember(dest => dest.TipoCliente, o => o.MapFrom(src => GetTipoCliente(src.TipoCliente)))
                .ForMember(dest => dest.TipoImpiego, o => o.MapFrom(src => GetTipoImpiego(src.TipoImpiego)))
                .ForMember(dest => dest.TipoSesso, o => o.MapFrom(src => GetTipoSesso(src.TipoSesso)))
                .ForMember(dest => dest.TipoTitolo, o => o.MapFrom(src => GetTipoTitolo(src.TipoTitolo)))
                .ForMember(d => d.Documenti, o => o.MapFrom(s => s.DocumentoCliente))
            ;
            CreateMap<AnagraficaSaveCommand, Cliente>();
            CreateMap<TagSaveCommand, Tag>()
                .ForMember(dest => dest.TagName, opt => opt.MapFrom(src => src.TagName.Trim()))
            ;
            CreateMap<DocumentoCliente, DocumentoModel>()
                .ForPath(d => d.Descrizione, o => o.MapFrom(s => s.Documento.Descrizione))
                .ForMember(d => d.NomeFile, o => o.MapFrom(s => s.Documento.NomeFile))
                .ForMember(d => d.Estensione, o => o.MapFrom(s => s.Documento.Estensione))
                .ForMember(d => d.NomeFileUtente, o => o.MapFrom(s => s.Documento.NomeFileUtente))
                .ForMember(d => d.Dimensione, o => o.MapFrom(s => s.Documento.Dimensione))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.Documento.Url))
            ;

            CreateMap(typeof(PagedList<>), typeof(PagedViewModel<>)).ConvertUsing(typeof(PagedModelConverter<,>));
        }

        private static ETipoCanalePreferito GetTipoCanale(ETipoCanalePreferito tipoCanale)
        {
            if (!Enum.IsDefined(typeof(ETipoCanalePreferito), tipoCanale))
                return ETipoCanalePreferito.NonSpecificato;
            else
                return tipoCanale;
        }

        private static EStatoCivile GetStatoCivile(EStatoCivile statoCivile)
        {
            if (!Enum.IsDefined(typeof(EStatoCivile), statoCivile))
                return EStatoCivile.NonSpecificato;
            else
                return statoCivile;
        }

        private static ETipoCliente GetTipoCliente(ETipoCliente tipoCliente)
        {
            if (!Enum.IsDefined(typeof(ETipoCliente), tipoCliente))
                return ETipoCliente.PersonaFisica;
            else
                return tipoCliente;
        }

        private static ETipoImpiego GetTipoImpiego(ETipoImpiego tipoImpiego)
        {
            if (!Enum.IsDefined(typeof(ETipoImpiego), tipoImpiego))
                return ETipoImpiego.NonSpecificato;
            else
                return tipoImpiego;
        }

        private static ETipoSesso GetTipoSesso(ETipoSesso tipoSesso)
        {
            if (!Enum.IsDefined(typeof(ETipoSesso), tipoSesso))
                return ETipoSesso.NonSpecificato;
            else
                return tipoSesso;
        }

        private static ETipoTitolo GetTipoTitolo(ETipoTitolo tipoTitolo)
        {
            if (!Enum.IsDefined(typeof(ETipoTitolo), tipoTitolo))
                return ETipoTitolo.NonSpecificato;
            else
                return tipoTitolo;
        }
    }
}
