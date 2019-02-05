using FluentValidation;

namespace Sfa.Tl.Matching.Application.FileReader.RoutePathMapping
{
    public class RoutePathMappingDataValidator : AbstractValidator<string[]>
    {
        public RoutePathMappingDataValidator()
        {
            //RuleFor(x => x.Id).NotNull();
            //RuleFor(x => x.Title).Length(0, 250);
            //RuleFor(x => x.ShortTitle).Length(0, 50);
        }
    }
}
