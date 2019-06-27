using System;
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
    public class When_EmployerService_Is_Called_To_GetEmployer
    {
        private readonly IRepository<Domain.Models.Employer> _employerRepository;
        private readonly EmployerStagingDto _employerStagingResult;

        private const int EmployerId = 1;

        public When_EmployerService_Is_Called_To_GetEmployer()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerStagingMapper).Assembly));
            var mapper = new Mapper(config);

            _employerRepository = Substitute.For<IRepository<Domain.Models.Employer>>();
            var opportunityRepository = Substitute.For<IOpportunityRepository>();

            var employer = new ValidEmployerBuilder().Build();

            _employerRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Employer, bool>>>())
                .Returns(employer);

            var employerService = new EmployerService(mapper, _employerRepository, opportunityRepository);

            _employerStagingResult = employerService.GetEmployer(EmployerId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetEmployer_Is_Called_Exactly_Once()
        {
            _employerRepository.Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Employer, bool>>>());
        }

        [Fact]
        public void Then_The_Employer_CrmId_Is_As_Expected()
        {
            _employerStagingResult.CrmId.Should().Be(new Guid("A3185517-2255-4299-ACCA-36116D512B46"));
        }

        [Fact]
        public void Then_The_Employer_Company_Name_Is_As_Expected()
        {
            _employerStagingResult.CompanyName.Should().Be("My Company");
        }

        [Fact]
        public void Then_The_Employer_CompanyType_Is_As_Expected()
        {
            _employerStagingResult.CompanyType.Should().Be("Employer");
        }

        [Fact]
        public void Then_The_Employer_Aupa_Is_As_Expected()
        {
            _employerStagingResult.Aupa.Should().Be("Active");
        }

        [Fact]
        public void Then_The_Employer_AlsoKnownAs_Is_As_Expected()
        {
            _employerStagingResult.AlsoKnownAs.Should().Be("Another Also Known As");
        }

        [Fact]
        public void Then_The_Employer_PostCode_Is_As_Expected()
        {
            _employerStagingResult.Postcode.Should().Be("AB1 1AA");
        }

        [Fact]
        public void Then_The_Employer_Owner_Is_As_Expected()
        {
            _employerStagingResult.Owner.Should().Be("Owner");
        }

        [Fact]
        public void Then_The_Employer_PrimaryContact_Is_As_Expected()
        {
            _employerStagingResult.PrimaryContact.Should().Be("Primary EmployerContact");
        }
        [Fact]
        public void Then_The_Employer_Phone_Is_As_Expected()
        {
            _employerStagingResult.Phone.Should().Be("01474777777");
        }

        [Fact]
        public void Then_The_Employer_Email_Is_As_Expected()
        {
            _employerStagingResult.Email.Should().Be("email@address.com");
        }
    }
}
