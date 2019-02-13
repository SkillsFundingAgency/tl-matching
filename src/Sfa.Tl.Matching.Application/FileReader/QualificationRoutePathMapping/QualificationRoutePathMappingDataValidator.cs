using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Humanizer;
using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.QualificationRoutePathMapping
{
    public class QualificationRoutePathMappingDataValidator : AbstractValidator<QualificationRoutePathMappingFileImportDto>
    {
        public const int MaximumTitleLength = 250;
        public const int MaximumShortTitleLength = 100;

        public IDictionary<int, string> PathMapping { get; set; }


        public QualificationRoutePathMappingDataValidator(IRepository<Domain.Models.RoutePathMapping> repository, IRoutePathRepository routePathRepository)
        {
            var paths = routePathRepository.GetPaths().ToList();

            PathMapping = paths
                .Select(p =>
                    new KeyValuePair<int, string>(p.Id,
                        p.Name
                            .Replace(" ", "")
                            .Replace(",", "")
                            .ToLower()))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            RuleFor(dto => dto)
                .Must(MustHaveAtLeastOnePathId)
                    .WithErrorCode(ValidationErrorCode.WrongNumberOfColumns.ToString())
                    .WithMessage(ValidationErrorCode.WrongNumberOfColumns.Humanize());

            RuleFor(dto => dto)
                .Must(PathIdValuesMustMatchColumnType)
                    .WithErrorCode(ValidationErrorCode.ColumnValueDoesNotMatchType.ToString())
                    .WithMessage(ValidationErrorCode.ColumnValueDoesNotMatchType.Humanize());

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
                    .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.Title)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Length(0, MaximumTitleLength)
                    .WithErrorCode(ValidationErrorCode.InvalidLength.ToString())
                    .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.Title)}' - {ValidationErrorCode.InvalidLength.Humanize()}");

            RuleFor(dto => dto.ShortTitle)
                .NotNull()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.ShortTitle)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.ShortTitle)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Length(0, MaximumShortTitleLength)
                    .WithErrorCode(ValidationErrorCode.InvalidLength.ToString())
                    .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.ShortTitle)}' - {ValidationErrorCode.InvalidLength.Humanize()}");

            RuleFor(dto => dto.Source)
                .NotNull()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.Source)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.Source)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
        }

        private async Task<bool> CanLarsIdBeAdded(IRepository<Domain.Models.RoutePathMapping> repository, string larsId)
        {
            var routePathMapping = await repository.GetMany(p => p.LarsId == larsId);

            return routePathMapping == null || !routePathMapping.Any();
        }

        private bool MustHaveAtLeastOnePathId(QualificationRoutePathMappingFileImportDto data)
        {
            var pathIds = data.GetQualificationRoutePathMappingPathIdColumns();
            return pathIds.Any();
        }

        private bool PathIdValuesMustMatchColumnType(QualificationRoutePathMappingFileImportDto data)
        {
            var pathIds = data.GetQualificationRoutePathMappingPathIdColumns();

            var isValid = true;

            foreach (var p in pathIds)
            {
                if (!int.TryParse(p.GetValue(data).ToString(), out var pathIdValue))
                {
                    isValid = false;
                }
                else if (PathMapping.TryGetValue(pathIdValue, out var path))
                {
                    if (path != p.Name.ToLower())
                    {
                        isValid = false;
                    }
                }
                else
                {
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}
