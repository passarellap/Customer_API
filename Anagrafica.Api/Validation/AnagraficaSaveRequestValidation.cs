using FluentValidation;
using AnagraficaAPI.Request;
using TreePlat.Domain.Enumerations;
using System;

namespace AnagraficaAPI.Validation
{
    public class AnagraficaSaveRequestValidation : AbstractValidator<AnagraficaSaveRequest>
    {
        public AnagraficaSaveRequestValidation()
        {
            When(x => x.IdIndirizzo != null, () => { RuleFor(x => x.IdIndirizzo).GreaterThan(0); });
            When(x => x.TipoCliente == ETipoCliente.PersonaFisica, () =>
            {
                RuleFor(x => x.Cognome)
                    .NotEmpty()
                    .Length(1, 50);

                RuleFor(x => x.Nome)
                    .Length(0, 50);
            }).Otherwise(() =>
            {
                RuleFor(x => x.RagioneSociale)
                    .NotEmpty()
                    .Length(1, 100);
            });

            When(x => x.IdCollaboratore != null, () => { RuleFor(x => x.IdCollaboratore).GreaterThan(0); });
            When(x => x.IdEditore != null, () => { RuleFor(x => x.IdCollaboratore).GreaterThan(0); });
            When(x => x.IdSettore != null, () => { RuleFor(x => x.IdCollaboratore).GreaterThan(0); });

            RuleFor(x => x.Telefono).Length(0, 30);
            RuleFor(x => x.Telefono2).Length(0, 30);
            RuleFor(x => x.Cellulare).Length(0, 30);
            RuleFor(x => x.Fax).Length(0, 30);

            When(x => string.IsNullOrEmpty(x.EMail) == false, () =>
            {
                RuleFor(x => x.EMail)
                    .Length(0, 320)
                    .EmailAddress();
            });

            When(x => string.IsNullOrEmpty(x.EMail2) == false, () =>
            {
                RuleFor(x => x.EMail2)
                    .Length(0, 320)
                    .EmailAddress();
            });
            RuleFor(x => x.SkypeId).Length(0, 320);
            RuleFor(x => x.AltriRecapiti).Length(0, 320);
            RuleFor(x => x.AutoritaLuogoRilascio).Length(0, 30);
            RuleFor(x => x.CodiceCRM).Length(0, 50);
            RuleFor(x => x.CodiceFiscale).Length(0, 16);
            RuleFor(x => x.NumeroDocumentoPresentato).Length(0, 15);
            RuleFor(x => x.Piva).Length(0, 11);
            RuleFor(x => x.CodicePadreCrm).Length(0, 50);
            RuleFor(x => x.NascitaLuogo).Length(0, 320);
            RuleFor(x => x.NascitaProvincia).Length(0, 320);

            When(x => x.EsenteAui != null, () => { RuleFor(x => x.EsenteAui).Must(x => x == false || x == true); });

            When(x => x.DataRilascioDocumento != null, () => { RuleFor(x => x.DataRilascioDocumento).Must(date => date != default(DateTime)); });
            When(x => x.DataScadenzaDocumento != null, () => { RuleFor(x => x.DataScadenzaDocumento).Must(date => date != default(DateTime)); });
            When(x => x.NascitaData != null, () => { RuleFor(x => x.NascitaData).Must(date => date != default(DateTime)); });

            RuleFor(x => x.TipoCliente).IsInEnum();
            RuleFor(x => x.TipoDocumentoPresentato).IsInEnum();
            When(x => (int)x.TipoCanalePreferito != 0, () => {
                RuleFor(x => x.TipoCanalePreferito).IsInEnum();
            });
        }
    }
}
