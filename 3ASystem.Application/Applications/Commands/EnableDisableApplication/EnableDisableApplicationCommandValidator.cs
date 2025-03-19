using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Application.Applications.Commands.EnableDisableApplication;

public class EnableDisableApplicationCommandValidator : AbstractValidator<EnableDisableApplicationCommand>
{
	public EnableDisableApplicationCommandValidator()
	{
		RuleFor(c => c.Id).NotEmpty();
	}

}
