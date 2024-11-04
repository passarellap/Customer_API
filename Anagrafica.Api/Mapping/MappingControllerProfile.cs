using AnagraficaAPI.Domain.Command;
using AnagraficaAPI.Domain.Model;
using AnagraficaAPI.Request;
using AnagraficaAPI.Response;
using AutoMapper;
using ControllerUtilities.Request;
using ControllerUtilities.Response;
using ControllerUtilities.String;
using ServiceUtilities.Model;
using ServiceUtilities.Querying;
using System;
using System.Collections.Generic;
using TreePlat.Domain.Enumerations;

namespace AnagraficaAPI.Mapping
{
    public class MappingControllerProfile : Profile
    {
        public MappingControllerProfile()
        {
            CreateMap<AnagraficaFilterRequest, AnagraficaFilterCommand>();
            CreateMap<UpdatedModel, BaseResponseDto>();
            CreateMap<IndirizzoModel, IndirizzoResponseDto>();
            CreateMap<AnagraficaModel, AnagraficaResponseDto>();
            CreateMap<TagModel, TagResponseDto>();
            CreateMap<DocumentoModel, DocumentoResponseDto>();
            CreateMap<AnagraficaSaveRequest, AnagraficaSaveCommand>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => MapNotNullTruncate(50, src.Nome)))
                .ForMember(dest => dest.Cognome, opt => opt.MapFrom(src => MapNotNullTruncate(50, src.Cognome)))
                .ForMember(dest => dest.RagioneSociale, opt => opt.MapFrom(src => MapNotNullTruncate(100, src.RagioneSociale)))
                .ForMember(dest => dest.IdIndirizzo, opt => opt.MapFrom(src => src.IdIndirizzo))
                .ForMember(dest => dest.IdEditore, opt => opt.Ignore())
                .ForMember(dest => dest.IdSettore, opt => opt.Ignore())
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => MapNotNullTruncate(30, src.Telefono)))
                .ForMember(dest => dest.Telefono2, opt => opt.MapFrom(src => MapNotNullTruncate(30, src.Telefono2)))
                .ForMember(dest => dest.Cellulare, opt => opt.MapFrom(src => MapNotNullTruncate(30, src.Cellulare)))
                .ForMember(dest => dest.Fax, opt => opt.MapFrom(src => MapNotNullTruncate(30, src.Fax)))
                .ForMember(dest => dest.EMail, opt => opt.MapFrom(src => MapNotNullTruncate(320, src.EMail)))
                .ForMember(dest => dest.EMail2, opt => opt.MapFrom(src => MapNotNullTruncate(320, src.EMail2)))
                .ForMember(dest => dest.SitoInternet, opt => opt.MapFrom(src => MapNotNullTruncate(200, src.SitoInternet)))
                .ForMember(dest => dest.SkypeId, opt => opt.MapFrom(src => MapNotNullTruncate(320, src.SkypeId)))
                .ForMember(dest => dest.AltriRecapiti, opt => opt.MapFrom(src => MapNotNullTruncate(200, src.AltriRecapiti)))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => MapNotNull(src.Note)))
                .ForMember(dest => dest.TipoConsenso, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.AutoritaLuogoRilascio, opt => opt.MapFrom(src => MapNotNullTruncate(30, src.AutoritaLuogoRilascio)))
                .ForMember(dest => dest.CodiceCRM, opt => opt.MapFrom(src => MapNotNullTruncate(50, src.CodiceCRM)))
                .ForMember(dest => dest.CodiceFiscale, opt => opt.MapFrom(src => MapNotNullTruncate(16, src.CodiceFiscale)))
                .ForMember(dest => dest.CodicePadre, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.CodicePadreCrm, opt => opt.MapFrom(src => MapNotNullTruncate(50, src.CodicePadreCrm)))
                .ForMember(dest => dest.DataRilascioDocumento, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.DataScadenzaDocumento, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Piva, opt => opt.MapFrom(src => MapNotNullTruncate(11, src.Piva)))
                .ForMember(dest => dest.Idorigine, opt => opt.MapFrom(src => string.Empty))
                .ForMember(dest => dest.NascitaLuogo, opt => opt.MapFrom(src => MapNotNullTruncate(30, src.NascitaLuogo)))
                .ForMember(dest => dest.NascitaProvincia, opt => opt.MapFrom(src => MapNotNullTruncate(30, src.NascitaProvincia)))
                .ForMember(dest => dest.NumeroDocumentoPresentato, opt => opt.MapFrom(src => MapNotNullTruncate(15, src.NumeroDocumentoPresentato)))
                .ForMember(dest => dest.DataRilascioDocumento, opt => opt.MapFrom(src => MapNotNull(src.DataRilascioDocumento)))
                .ForMember(dest => dest.DataScadenzaDocumento, opt => opt.MapFrom(src => MapNotNull(src.DataScadenzaDocumento)))
                .ForMember(dest => dest.EsenteAui, opt => opt.MapFrom(src => MapNotNull(src.EsenteAui)))
                .ForMember(dest => dest.TipoCanalePreferito, opt => opt.MapFrom(src => src.TipoCanalePreferito == 0 ? ETipoCanalePreferito.NonSpecificato : src.TipoCanalePreferito))
                .ForMember(dest => dest.StatoCivile, opt => opt.MapFrom(src => src.StatoCivile == 0 ? EStatoCivile.NonSpecificato : src.StatoCivile))
                .ForMember(dest => dest.TipoImpiego, opt => opt.MapFrom(src => src.TipoImpiego == 0 ? ETipoImpiego.NonSpecificato : src.TipoImpiego))
                .ForMember(dest => dest.TipoSesso, opt => opt.MapFrom(src => src.TipoSesso == 0 ? ETipoSesso.NonSpecificato : src.TipoSesso))
                .ForMember(dest => dest.TipoTitolo, opt => opt.MapFrom(src => src.TipoTitolo == 0 ? ETipoTitolo.NonSpecificato : src.TipoTitolo))
                .ForMember(dest => dest.TagCliente, opt => opt.MapFrom(src => 0))
            ;
            CreateMap<TagSaveRequest, TagSaveCommand>()
                .ForMember(dest => dest.TagName, opt => opt.MapFrom(src => MapNotNullTruncate(50, src.TagName)))
            ;
            CreateMap<IndirizzoSaveRequest, IndirizzoSaveCommand>()
                .ForMember(dest => dest.Descrizione, opt => opt.MapFrom(src => MapNotNullTruncate(50, src.Descrizione)))
                .ForMember(dest => dest.Civico, opt => opt.MapFrom(src => MapNotNullTruncate(10, src.Civico)))
                .ForMember(dest => dest.Latitudine, opt => opt.MapFrom(src => MapNotNull(src.Latitudine)))
                .ForMember(dest => dest.Longitudine, opt => opt.MapFrom(src => MapNotNull(src.Longitudine)))
                .ForMember(dest => dest.Cap, opt => opt.MapFrom(src => MapNotNullTruncate(50, src.Cap)))
            ;
            CreateMap<DocumentoSaveRequest, DocumentoSaveCommand>()
               .ForMember(dest => dest.Descrizione, opt => opt.MapFrom(src => MapNotNullTruncate(50, src.Descrizione)));

            CreateMap<SystemInfoModel, InfoResponseDto>();
            CreateMap<QueryStringParametersRequest, QueryStringParameters>();
            CreateMap(typeof(PagedViewModel<>), typeof(PagedViewDto<>)).ConvertUsing(typeof(PagedDtoConverter<,>));
        }

        private string MapNotNull(string data) => data ?? string.Empty;
        private DateTime MapNotNull(DateTime? data) => data == null ? new DateTime(1753, 1, 1) : data.Value;
        private bool MapNotNull(bool? data) => data != null && data.Value;
        private decimal MapNotNull(decimal? data) => data == null ? 0 : data.Value;
        private string MapNotNullTruncate(int len, string data = null)
        {
            if (data != null)
                return data.TruncateString(len);
            else
                return string.Empty;
        }
    }
}
