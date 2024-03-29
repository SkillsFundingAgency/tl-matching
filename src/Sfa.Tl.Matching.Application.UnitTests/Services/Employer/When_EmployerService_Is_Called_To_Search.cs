﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Event;
using Sfa.Tl.Matching.Tests.Common.Extensions;
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
            var employerRepository = Substitute.For<IRepository<Domain.Models.Employer>>();
            var opportunityRepository = Substitute.For<IOpportunityRepository>();

            employerRepository.GetMany(Arg.Any<Expression<Func<Domain.Models.Employer, bool>>>())
                .Returns(new SearchResultsBuilder().Build().AsQueryable());

            var employerService = new EmployerService(employerRepository, opportunityRepository, AutoMapperExtension.GetRealMapper(), Substitute.For<IValidator<CrmEmployerEventBase>>(),
                Substitute.For<IMessageQueueService>());

            const string companyName = "Co";

            _searchResults = employerService.Search(companyName).ToList();

            _firstEmployer = _searchResults[0];
            _secondEmployer = _searchResults[1];
            _thirdEmployer = _searchResults[2];
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Search_Results_Is_Returned()
        {
            _searchResults.Count.Should().Be(3);
        }

        [Fact]
        public void Then_The_First_Employer_Is_In_Correct_Order_With_Expected_Values()
        {
            _firstEmployer.CompanyName.Should().Be("Another Company");
            _firstEmployer.AlsoKnownAs.Should().Be("Another Also Known As");
        }

        [Fact]
        public void Then_The_Second_Employer_Is_In_Correct_Order_With_Expected_Values()
        {
            _secondEmployer.CompanyName.Should().Be("Company");
            _secondEmployer.AlsoKnownAs.Should().Be("Also Known As");
        }

        [Fact]
        public void Then_The_Third_Employer_Is_In_Correct_Order_With_Expected_Values()
        {
            _thirdEmployer.CompanyName.Should().Be("Z Company");
            _thirdEmployer.AlsoKnownAs.Should().Be("Z Also Known As");
        }
    }
}