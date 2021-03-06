﻿using System.IO;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Mappers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Mappers
{
    public class When_DataImportViewModelMapper_Maps_ViewModel
    {
        private readonly DataUploadDto _result;

        private const string FormattedDateTimeNowUtc = "201902011011120000000KZ";
        private const string FileName = "TestFile";
        private const string FileContentType = "text";
        private readonly byte[] _data;

        public When_DataImportViewModelMapper_Maps_ViewModel()
        {
            _data = new byte[] { 0x48, 0x45, 0x4C, 0x4C, 0x4F };

            var formFile = Substitute.For<IFormFile>();
            formFile.FileName.Returns(FileName);
            formFile.ContentType.Returns(FileContentType);

            formFile.CopyTo(Arg.Do<MemoryStream>(ms =>
            {
                ms.Write(_data);
                ms.Seek(0, SeekOrigin.Begin);
            }));

            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            dateTimeProvider.UtcNowString(Arg.Any<string>()).Returns(FormattedDateTimeNowUtc);

            var viewModel = new DataImportParametersViewModel
            {
                File = formFile,
                IsImportSuccessful = true,
                SelectedImportType = DataImportType.LearningAimReference
            };

            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<DataImportViewModelMapper>();
                c.ConstructServicesUsing(d => new FileNameResolver(dateTimeProvider));
            });

            IMapper mapper = new Mapper(config);
            _result = mapper.Map<DataUploadDto>(viewModel);
        }

        [Fact]
        public void Then_Result_Should_Be_Mapped_Correctly()
        {
            _result.Should().NotBeNull();
            _result.FileName.Should().Be($"{FormattedDateTimeNowUtc}{FileName}");
            _result.ContentType.Should().Be(FileContentType);
            _result.ImportType.Should().Be(DataImportType.LearningAimReference);
            _result.Data.Should().BeEquivalentTo(_data);
        }
    }
}
