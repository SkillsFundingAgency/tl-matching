using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_Controller_Delete_Employer
    {

        [Theory, AutoDomainData]
        public async Task Then_Result_Should_Return_To_GetSavedEmployerOpportunity(
                                                    SavedEmployerOpportunityViewModel viewModel,
                                                    IEmployerService employerService,
                                                    EmployerController sut,
                                                    int opportunityId
                                                    )
        {
            //Arrange
            employerService.GetSavedEmployerOpportunitiesAsync(Arg.Any<string>()).Returns(viewModel);

            //Act
            var result = await sut.DeleteEmployer(opportunityId) as RedirectToRouteResult;

            //Assert
            result.Should().NotBeNull();
            result?.RouteName.Should().Be("GetSavedEmployerOpportunity");
            result?.RouteName.Should().NotBe("Start");

        }

        [Theory, AutoDomainData]
        public async Task Then_Result_Should_Return_To_Start(
                                                    [Frozen] IEmployerService employerService,
                                                    EmployerController sut,
                                                    int opportunityId
                                                    )
        {
            //Arrange
            employerService.GetSavedEmployerOpportunitiesAsync(Arg.Any<string>()).Returns(new SavedEmployerOpportunityViewModel());

            //Act
            var result = await sut.DeleteEmployer(opportunityId) as RedirectToRouteResult;

            //Assert
            result.Should().NotBeNull();
            result?.RouteName.Should().Be("Start");
            result?.RouteName.Should().NotBe("GetSavedEmployerOpportunity");
        }

    }
}
