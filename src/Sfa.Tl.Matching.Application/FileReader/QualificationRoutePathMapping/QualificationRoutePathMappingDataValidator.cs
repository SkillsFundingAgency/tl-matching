using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Humanizer;
using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.QualificationRoutePathMapping
{
    public class QualificationRoutePathMappingDataValidator : AbstractValidator<QualificationRoutePathMappingFileImportDto>
    {
        public const int MaximumTitleLength = 250;
        public const int MaximumShortTitleLength = 100;

        public IDictionary<int, string> PathMapping { get; set; }

        public QualificationRoutePathMappingDataValidator(IRepository<Domain.Models.QualificationRoutePathMapping> qualificationRoutePathMappingRepository, IRepository<Qualification> qualificationRepository, IRepository<Path> pathRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            var paths = pathRepository.GetMany();

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
                    .WithMessage(ValidationErrorCode.WrongNumberOfColumns.Humanize())
                //We Must have at least 1 PathId in Row otherwise there is no Point processing other rules
                .DependentRules(() =>
                {

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
                        //LarsId Must be valid Before we can check related Qualification in Qualification Table
                        .DependentRules(() =>
                        {
                            RuleFor(dto => dto)
                                .MustAsync((dto, cancellation) => BeUniqueueQualificationRoutePathMapping(qualificationRoutePathMappingRepository, qualificationRepository, dto))
                                    .WithErrorCode(ValidationErrorCode.QualificationRoutePathMappingAlreadyExists.ToString())
                                    .WithMessage($"'{nameof(QualificationRoutePathMappingFileImportDto.LarsId)}' - {ValidationErrorCode.QualificationRoutePathMappingAlreadyExists.Humanize()}");
                        });
                });

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

        private async Task<bool> BeUniqueueQualificationRoutePathMapping(IRepository<Domain.Models.QualificationRoutePathMapping> routePathMappingRepository, IRepository<Qualification> qualificationRepository,  QualificationRoutePathMappingFileImportDto data)
        {
            var pathIds = data.GetQualificationRoutePathMappingPathIdColumnProperties().Select(p => int.Parse(p.GetValue(data).ToString()));
            
            //check if we are adding mapping for an existing qualification
            var qualification = await qualificationRepository.GetSingleOrDefault(q => q.LarsId == data.LarsId);

            if (qualification != null)
            {
                //if qualification exists then check if mapping already exists for this qualification path combination
                data.QualificationId = qualification.Id;

                var routePathMapping = routePathMappingRepository.GetMany(p => pathIds.Contains(p.PathId) && p.QualificationId == data.QualificationId);
            
                return !routePathMapping.Any();
            }

            //if there is no existing qualification then the route path map cant exists without it therefore the new qualification route path mapping will be definatly uniqueu
            return true;
        }

        private static bool MustHaveAtLeastOnePathId(QualificationRoutePathMappingFileImportDto data)
        {
            var pathIds = data.GetQualificationRoutePathMappingPathIdColumnProperties();
            return pathIds.Any();
        }

        private bool PathIdValuesMustMatchColumnType(QualificationRoutePathMappingFileImportDto data)
        {
            var pathIds = data.GetQualificationRoutePathMappingPathIdColumnProperties();

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
