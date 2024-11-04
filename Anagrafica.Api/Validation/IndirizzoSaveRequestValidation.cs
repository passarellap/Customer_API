using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnagraficaAPI.Request;
using FluentValidation;

namespace AnagraficaAPI.Validation
{
    public class IndirizzoSaveRequestValidation : AbstractValidator<IndirizzoSaveRequest>
    {
        public IndirizzoSaveRequestValidation()
        {
            RuleFor(x => x.IdComune).NotNull().GreaterThan(0);
        }
    }
}
