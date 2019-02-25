using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer
{
    [Trait("EmployerService", "Search for employers")]
    public class When_EmployerService_Is_Called_To_Search
    {
        private readonly List<EmployerSearchResultDto> _searchResults;

        private readonly EmployerSearchResultDto _firstEmployer;
        private readonly EmployerSearchResultDto _secondEmployer;
        private readonly EmployerSearchResultDto _thirdEmployer;

        public When_EmployerService_Is_Called_To_Search()
        {
            var config = new MapperConfiguration(c => c.AddProfile<EmployerMapper>());
            var mapper = new Mapper(config);
            var repository = Substitute.For<IRepository<Domain.Models.Employer>>();

            repository.GetMany(Arg.Any<Func<Domain.Models.Employer, bool>>())
                .Returns(new SearchResultsBuilder().Build().AsQueryable());

            var employerService = new EmployerService(mapper, null, repository);

            const string employerName = "Co";

            _searchResults = employerService.Search(employerName).GetAwaiter().GetResult().ToList();

            _firstEmployer = _searchResults[0];
            _secondEmployer = _searchResults[1];
            _thirdEmployer = _searchResults[2];
        }

        [Fact]
        public void Then_Correct_Number_Of_Results()
        {
            _searchResults.Count().Should().Be(3);
        }

        [Fact]
        public void Then_The_First_Employer_Is_In_Correct_Order()
        {
            _firstEmployer.EmployerNameWithAka.Should().Be("Another Company (Another Also Known As)");
        }

        [Fact]
        public void Then_The_Second_Employer_Is_In_Correct_Order()
        {
            _secondEmployer.EmployerNameWithAka.Should().Be("Company (Also Known As)");
        }

        [Fact]
        public void Then_The_Third_Employer_Is_In_Correct_Order()
        {
            _thirdEmployer.EmployerNameWithAka.Should().Be("Z Company (Z Also Known As)");
        }
    }
}