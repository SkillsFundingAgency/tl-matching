using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_Confirm_Remove_Employer
    {

        [Theory, AutoDomainData]
        public async Task Then_Result_Is_Not_Null(
                                            Domain.Models.Opportunity opportunity,
                                            IEmployerService employerService,
                                            EmployerController sut,
                                            [Frozen] RemoveEmployerDto dto)
        {
            //Arrange
            employerService.GetConfirmDeleteEmployerOpportunityAsync(Arg.Any<int>(), Arg.Any<string>()).Returns(dto);

            //Act
            var result = await sut.ConfirmDeleteAsync(opportunity.Id) as ViewResult;

            //Assert
            result.Should().NotBeNull();

        }

        [Theory, AutoDomainData]
        public async Task Then_View_Result_Is_Returned(
                                                Domain.Models.Opportunity opportunity,
                                                IEmployerService employerService,
                                                EmployerController sut,
                                                [Frozen] RemoveEmployerDto dto
                                                )
        {
            //Arrange
            employerService.GetConfirmDeleteEmployerOpportunityAsync(Arg.Any<int>(), Arg.Any<string>()).Returns(dto);

            //Act
            var result = await sut.ConfirmDeleteAsync(opportunity.Id) as ViewResult;

            //Assert
            result?.Model.Should().BeOfType<RemoveEmployerViewModel>();

        }

        [Theory, AutoDomainData]
        public async Task Then_Confirm_Remove_Employer_Model_Is_Loaded(
                                                Domain.Models.Opportunity opportunity,
                                                IEmployerService employerService,
                                                EmployerController sut,
                                                [Frozen] RemoveEmployerDto dto
                                                )
        {
            //Arrange
            employerService.GetConfirmDeleteEmployerOpportunityAsync(Arg.Any<int>(), Arg.Any<string>()).Returns(dto);

            //Act
            var result = await sut.ConfirmDeleteAsync(opportunity.Id) as ViewResult;

            //Assert
            var viewModel = result?.Model as RemoveEmployerViewModel;

            viewModel.Should().NotBeNull();
            viewModel.OpportunityId.Should().Be(opportunity.Id);
            viewModel.ConfirmDeleteText.Should()
                .Be($"Confirm you want to delete {dto.OpportunityCount} opportunities created for {dto.EmployerName}");

            viewModel.ConfirmDeleteText.Should()
                .NotBe($"Confirm you want to delete {dto.OpportunityCount} opportunity created for {dto.EmployerName}");

            viewModel.WarningDeleteText.Should().Be("This cannot be undone.");
            viewModel.EmployerCount.Should().Be(dto.EmployerCount);
        }

        [Theory, AutoDomainData]
        public async Task Then_Confirm_Remove_Employer_Model_Is_Loaded_With_No_Employer(
                                                Domain.Models.Opportunity opportunity,
                                                IEmployerService employerService,
                                                EmployerController sut,
                                                [Frozen] RemoveEmployerDto dto
                                                )
        {
            //Arrange
            dto.OpportunityCount = 1;
            dto.EmployerCount = 1;

            employerService.GetConfirmDeleteEmployerOpportunityAsync(Arg.Any<int>(), Arg.Any<string>()).Returns(dto);
            
            //Act
            var result = await sut.ConfirmDeleteAsync(opportunity.Id) as ViewResult;

            //Assert
            var viewModel = result.GetViewModel<RemoveEmployerViewModel>();

            viewModel.OpportunityId.Should().Be(opportunity.Id);
            viewModel.ConfirmDeleteText.Should()
                .Be($"Confirm you want to delete {dto.OpportunityCount} opportunity created for {dto.EmployerName}");
            viewModel.WarningDeleteText.Should().Be("This cannot be undone and will mean you have no more employers with saved opportunities.");
            viewModel.SubmitActionText.Should().Be("Confirm and finish");
            viewModel.EmployerCount.Should().Be(dto.EmployerCount);
        }
    }
}