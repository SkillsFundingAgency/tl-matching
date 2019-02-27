using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

            repository.GetMany(Arg.Any<Expression<Func<Domain.Models.Employer, bool>>>())
                .Returns(new SearchResultsBuilder().Build().AsQueryable());

            var employerService = new EmployerService(mapper, repository);

            const string employerName = "Co";

            _searchResults = employerService.Search(employerName).ToList();

            _firstEmployer = _searchResults[0];
            _secondEmployer = _searchResults[1];
            _thirdEmployer = _searchResults[2];
        }

        [Fact]
        public void Then_Correct_Number_Of_Results()
        {
            _searchResults.Count.Should().Be(3);
        }

        [Fact]
        public void Then_The_First_Employer_Is_In_Correct_Order_With_EmployerName()
        {
            _firstEmployer.EmployerName.Should().Be("Another Company");
        }


        [Fact]
        public void Then_The_First_Employer_Is_In_Correct_Order_With_AlsoKnownAs()
        {
            _firstEmployer.AlsoKnownAs.Should().Be("Another Also Known As");
        }

        [Fact]
        public void Then_The_Second_Employer_Is_In_Correct_Order_With_EmployerName()
        {
            _secondEmployer.EmployerName.Should().Be("Company");
        }

        [Fact]
        public void Then_The_Second_Employer_Is_In_Correct_Order_With_AlsoKnownAs()
        {
            _secondEmployer.AlsoKnownAs.Should().Be("Also Known As");
        }

        [Fact]
        public void Then_The_Third_Employer_Is_In_Correct_Order_With_EmployerName()
        {
            _thirdEmployer.EmployerName.Should().Be("Z Company");
        }

        [Fact]
        public void Then_The_Third_Employer_Is_In_Correct_Order_With_AlsoKnownAs()
        {
            _thirdEmployer.AlsoKnownAs.Should().Be("Z Also Known As");
        }
    }
}