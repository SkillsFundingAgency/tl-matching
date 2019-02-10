using System.Threading.Tasks;
using FluentValidation;
using Humanizer;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.RoutePathMapping
{
    public class RoutePathMappingDataValidator : AbstractValidator<string[]>
    {
        private const int MinimumNumberOfColumns = 4;

        public RoutePathMappingDataValidator(IRepository<Domain.Models.RoutePathMapping> repository)
        {
            //RuleFor(x => x.Id).NotNull();
            //RuleFor(x => x.Title).Length(0, 250);
            //RuleFor(x => x.ShortTitle).Length(0, 50);
            When(x => x.Length >= MinimumNumberOfColumns, () =>
            {
                RuleFor(x => x[(int) RoutePathMappingColumnIndex.LarsId])
                    .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage(
                        $"'{nameof(RoutePathMappingColumnIndex.LarsId)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                    //.Matches(ValidationConstants.UkprnRegex)
                    //.WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    //.WithMessage(
                    //    $"'{nameof(RoutePathMappingColumnIndex.LarsId)}' - {ValidationErrorCode.InvalidFormat.Humanize()}")
                    .MustAsync((x, cancellation) => CanLarsIdBeAdded(repository, x))
                    .WithErrorCode(ValidationErrorCode.RecordExists.ToString())
                    .WithMessage(
                        $"'{nameof(RoutePathMappingColumnIndex.LarsId)}' - {ValidationErrorCode.RecordExists.Humanize()}");
            });
        }

        private async Task<bool> CanLarsIdBeAdded(IRepository<Domain.Models.RoutePathMapping> repository, string larsId)
        {
            var routePathMapping = await repository.GetSingleOrDefault(p => p.LarsId == larsId);

            return routePathMapping == null;
        }
    }
}
