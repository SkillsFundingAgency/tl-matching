using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation;
using Humanizer;
using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.RoutePathMapping
{
    public class QualificationRoutePathMappingDataValidator : AbstractValidator<QualificationRoutePathMappingFileImportDto>
    {
        public const int MaximumTitleLength = 250;
        public const int MaximumShortTitleLength = 100;

        public QualificationRoutePathMappingDataValidator(IRepository<Domain.Models.RoutePathMapping> repository)
        {
            RuleFor(dto => dto)
                .Must(MustHaveAtLeastOnePathId)
                .WithErrorCode(ValidationErrorCode.WrongNumberOfColumns.ToString())
                .WithMessage(ValidationErrorCode.WrongNumberOfColumns.Humanize());

            RuleFor(dto => dto.LarsId)
                    .NotNull()
                        .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                        .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.LarsId)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                    .NotEmpty()
                        .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                        .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.LarsId)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                    .Matches(ValidationConstants.LarsIdRegex)
                        .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                        .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.LarsId)}' - {ValidationErrorCode.InvalidFormat.Humanize()}")
                    .MustAsync((x, cancellation) => CanLarsIdBeAdded(repository, x))
                        .WithErrorCode(ValidationErrorCode.RecordAlreadyExists.ToString())
                        .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.LarsId)}' - {ValidationErrorCode.RecordAlreadyExists.Humanize()}");

            RuleFor(dto => dto.Title)
                .NotNull()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.Title)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.Title)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

            RuleFor(dto => dto.ShortTitle)
                .NotNull()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.ShortTitle)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.ShortTitle)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

            RuleFor(dto => dto.Title).Length(0, MaximumTitleLength)
                .WithErrorCode(ValidationErrorCode.InvalidLength.ToString())
                .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.Title)}' - {ValidationErrorCode.InvalidLength.Humanize()}");

            RuleFor(dto => dto.ShortTitle).Length(0, MaximumShortTitleLength)
                .WithErrorCode(ValidationErrorCode.InvalidLength.ToString())
                .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.ShortTitle)}' - {ValidationErrorCode.InvalidLength.Humanize()}");
        }

        private async Task<bool> CanLarsIdBeAdded(IRepository<Domain.Models.RoutePathMapping> repository, string larsId)
        {
            var routePathMapping = await repository.GetMany(p => p.LarsId == larsId);

            return routePathMapping == null || !routePathMapping.Any();
        }

        private bool MustHaveAtLeastOnePathId(QualificationRoutePathMappingFileImportDto data)
        {
            var pathIds = data.GetType().GetProperties()
                .Where(pr => pr.GetCustomAttribute<ColumnAttribute>() != null)
                .SkipWhile(info => info.Name == nameof(QualificationRoutePathMappingFileImportDto.LarsId) ||
                                   info.Name == nameof(QualificationRoutePathMappingFileImportDto.Title) ||
                                   info.Name == nameof(QualificationRoutePathMappingFileImportDto.ShortTitle))
                .Where(pr => pr.GetValue(data) != null && !string.IsNullOrWhiteSpace(pr.GetValue(data).ToString())).ToList();

            return pathIds.Any();
        }
    }
}
