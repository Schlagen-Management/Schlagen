using FluentValidation;
using Schlagen.Data.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schlagen.Data.Validators
{
    public class InformationRequestValidator : AbstractValidator<InformationRequest>
    {
        public InformationRequestValidator()
        {
            RuleFor(ir => ir.Name)
                .NotEmpty()
                .WithMessage("Provide a name");

            RuleFor(ir => ir.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Provide a valid email address");

            RuleFor(ir => ir.Phone)
                .NotEmpty()
                .WithMessage("Provide a phone number");

            RuleFor(ir => ir.Details)
                .NotEmpty()
                .WithMessage("Provide a detailed information regarding your request");
        }
    }
}
