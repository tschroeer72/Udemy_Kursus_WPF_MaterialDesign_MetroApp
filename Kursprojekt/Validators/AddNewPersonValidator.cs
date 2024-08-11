using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikkiDBLib.Modells;

namespace Kursprojekt.Validators
{
    public class AddNewPersonValidator : AbstractValidator<Person>
    {
        public AddNewPersonValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name darf nicht leer sein");
            RuleFor(p => p.Vorname).NotEmpty().WithMessage("Vorname darf nicht leer sein");
            RuleFor(p => p.Bild).NotNull().WithMessage("Bild muss geladen werden");
            RuleFor(p => p.StadtID).GreaterThan(-1).WithMessage("Stadt darf nicht leer sein");
        }
    }
}
