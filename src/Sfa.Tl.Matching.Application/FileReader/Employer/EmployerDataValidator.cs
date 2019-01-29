using FluentValidation;

namespace Sfa.Tl.Matching.Application.FileReader.Employer
{
    public class EmployerDataValidator : AbstractValidator<string[]>
    {
        public EmployerDataValidator()
        {
            //RuleFor(x => x.Id).NotNull();
            //RuleFor(x => x.Name).Length(0, 10);
            //RuleFor(x => x.Email).EmailAddress();
            //RuleFor(x => x.Age).InclusiveBetween(18, 60);
        }
    }
}
