using System.Security.Claims;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_SaveOpportunityCompanyName_Is_Submitted_And_Employer_Is_Locked_By_Current_User
    {
        private readonly EmployerController _employerController;

        public When_Employer_SaveOpportunityCompanyName_Is_Submitted_And_Employer_Is_Locked_By_Current_User()
        {
            var employerService = Substitute.For<IEmployerService>();
            employerService.ValidateCompanyNameAndId(Arg.Any<int>(), Arg.Any<string>())
                .Returns(true);
            employerService.GetEmployerOpportunityOwnerAsync(Arg.Any<int>())
                .Returns("Current User");
            var opportunityService = Substitute.For<IOpportunityService>();

            var viewModel = new FindEmployerViewModel
            {
                OpportunityId = 1,
                CompanyName = "Company Name"
            };

            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(EmployerDtoMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<FindEmployerViewModel, CompanyNameDto>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<FindEmployerViewModel, CompanyNameDto>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<FindEmployerViewModel, CompanyNameDto>(new DateTimeProvider()) :
                                null);
            });

            var mapper = new Mapper(config);

            _employerController = new EmployerController(employerService, opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<EmployerController>(_employerController)
                .Add(ClaimTypes.Role, RolesExtensions.StandardUser)
                .AddUserName("Current User")
                .Build();

            controllerWithClaims.SaveOpportunityCompanyName(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Model_State_Has_No_Errors() =>
            _employerController.ViewData.ModelState.Should().BeEmpty();
    }
}