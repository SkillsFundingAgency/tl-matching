using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ExcelFileReader.Trim
{
    public class When_Validate_And_Parse_File_Is_Called_For_Valid_QualificationRoutePathMapping_File_Trim_Spaces : IClassFixture<ExcelFileReaderTrimDataTestFixture<QualificationRoutePathMappingFileImportDto, QualificationRoutePathMappingDto>>
    {
        private readonly ExcelFileReaderTrimDataTestFixture<QualificationRoutePathMappingFileImportDto, QualificationRoutePathMappingDto> _fixture;

        public When_Validate_And_Parse_File_Is_Called_For_Valid_QualificationRoutePathMapping_File_Trim_Spaces(ExcelFileReaderTrimDataTestFixture<QualificationRoutePathMappingFileImportDto, QualificationRoutePathMappingDto> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Then_Data_Validator_Validate_Is_called_Exactly_Once_And_Leading_And_Trailing_Speaces_are_Trimmed()
        {
            _fixture._dataValidator.Received(1).ValidateAsync(Arg.Is<QualificationRoutePathMappingFileImportDto>(arg =>
                arg.LarsId == "10000010" && 
                arg.Title == "Level 3 Advanced GCE in Art and Design" && 
                arg.ShortTitle == "L3 AGCE Art and Design" &&
                arg.CraftandDesign == "10" &&
                arg.MediaBroadcastandProduction == "12" &&
                arg.Marketing == "31" &&
                arg.Source == "Gatsby"
                ));
        }

        [Fact]
        public void Then_FunctionLog_Repository_Create_Many_Is_Called_With_Empty_List()
        {
            _fixture._functionLogRepository.Received(1).CreateMany(Arg.Is<List<FunctionLog>>(logs => logs.Count == 0));
        }

        [Fact]
        public void Then_Data_Parser_Parse_Is_Called_Exactly_Once()
        {
            _fixture._dataParser.Received(1).Parse(Arg.Any<QualificationRoutePathMappingFileImportDto>());
        }

        [Fact]
        public void Then_Returned_List_Has_Three_Item()
        {
            _fixture._results.Should().NotBeNullOrEmpty();

            _fixture._results.Count.Should().Be(3);

            var dto1 = _fixture._results.ElementAt(0);
            dto1.QualificationId.Should().Be(0);
            dto1.PathId.Should().Be(10);
            dto1.Source.Should().Be("Gatsby");

            var dto2 = _fixture._results.ElementAt(1);
            dto2.QualificationId.Should().Be(0);
            dto2.PathId.Should().Be(12);
            dto2.Source.Should().Be("Gatsby");

            var dto3 = _fixture._results.ElementAt(2);
            dto3.QualificationId.Should().Be(0);
            dto3.PathId.Should().Be(31);
            dto3.Source.Should().Be("Gatsby");
        }
    }
}