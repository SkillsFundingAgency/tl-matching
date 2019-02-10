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
        private const int MaximumTitleLength = 250;
        private const int MaximumShortTitleLength = 50;

        public RoutePathMappingDataValidator(IRepository<Domain.Models.RoutePathMapping> repository)
        {
            RuleFor(x => x)
                .Must(x => x.Length >= MinimumNumberOfColumns)
                .WithErrorCode(ValidationErrorCode.WrongNumberOfColumns.ToString())
                .WithMessage(ValidationErrorCode.WrongNumberOfColumns.Humanize());

            When(x => x.Length >= MinimumNumberOfColumns, () =>
            {
                RuleFor(x => x[(int) RoutePathMappingColumnIndex.LarsId])
                    .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage(
                        $"'{nameof(RoutePathMappingColumnIndex.LarsId)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                    
                    .Matches(ValidationConstants.LarsIdRegex)
                    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    .WithMessage(
                        $"'{nameof(RoutePathMappingColumnIndex.LarsId)}' - {ValidationErrorCode.InvalidFormat.Humanize()}")
                    
                    .MustAsync((x, cancellation) => CanLarsIdBeAdded(repository, x))
                    .WithErrorCode(ValidationErrorCode.RecordExists.ToString())
                    .WithMessage(
                        $"'{nameof(RoutePathMappingColumnIndex.LarsId)}' - {ValidationErrorCode.RecordExists.Humanize()}");

                RuleFor(x => x[RoutePathMappingColumnIndex.Title])
                    .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage(
                        $"'{nameof(RoutePathMappingColumnIndex.Title)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

                RuleFor(x => x[RoutePathMappingColumnIndex.Title]).Length(0, MaximumTitleLength)
                    .WithErrorCode(ValidationErrorCode.InvalidLength.ToString())
                    .WithMessage(
                        $"'{nameof(RoutePathMappingColumnIndex.Title)}' - {ValidationErrorCode.InvalidLength.Humanize()}");

                RuleFor(x => x[RoutePathMappingColumnIndex.ShortTitle]).Length(0, MaximumShortTitleLength)
                    .WithErrorCode(ValidationErrorCode.InvalidLength.ToString())
                    .WithMessage(
                        $"'{nameof(RoutePathMappingColumnIndex.ShortTitle)}' - {ValidationErrorCode.InvalidLength.Humanize()}");

            });
        }

        private async Task<bool> CanLarsIdBeAdded(IRepository<Domain.Models.RoutePathMapping> repository, string larsId)
        {
            var routePathMapping = await repository.GetSingleOrDefault(p => p.LarsId == larsId);

            return routePathMapping == null;
        }
    }
}
