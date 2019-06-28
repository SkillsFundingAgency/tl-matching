using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
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
    public class When_Employer_SaveOpportunityEmployerName_Is_Submitted_Successfully
    {
        private readonly IEmployerService _employerService;
        private readonly IOpportunityService _opportunityService;
        private const string EmployerName = "EmployerName";
        private const string ModifiedBy = "ModifiedBy";
        private readonly FindEmployerViewModel _viewModel = new FindEmployerViewModel();

        private const int OpportunityId = 1;
        private const int EmployerId = 2;

        public When_Employer_SaveOpportunityEmployerName_Is_Submitted_Successfully()
        {
            _viewModel.OpportunityId = OpportunityId;
            _viewModel.CompanyName = EmployerName;
            _viewModel.SelectedEmployerId = 2;

            _employerService = Substitute.For<IEmployerService>();
            _employerService.ValidateEmployerNameAndId(EmployerId, EmployerName).Returns(true);

            _opportunityService = Substitute.For<IOpportunityService>();

            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(EmployerDtoMapper).Assembly);
                c.ConstructServicesUsing(type => 
                    type.Name.Contains("LoggedInUserEmailResolver") ? 
                        new LoggedInUserEmailResolver<FindEmployerViewModel, EmployerNameDto>(httpcontextAccesor) :
                            type.Name.Contains("LoggedInUserNameResolver") ? 
                                (object) new LoggedInUserNameResolver<FindEmployerViewModel, EmployerNameDto>(httpcontextAccesor) :
                                    type.Name.Contains("UtcNowResolver") ? 
                                        new UtcNowResolver<FindEmployerViewModel, EmployerNameDto>(new DateTimeProvider()) :
                                            null);
            });

            var mapper = new Mapper(config);

            var employerController = new EmployerController(_employerService, _opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<EmployerController>(employerController)
                .AddUserName(ModifiedBy)
                .Build();

            httpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);
            
            controllerWithClaims.SaveOpportunityEmployerName(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetEmployer_Is_Called_Exactly_Once()
        {
            _employerService.Received(1).ValidateEmployerNameAndId(EmployerId, EmployerName);
        }

        [Fact]
        public void Then_UpdateOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).UpdateOpportunity(Arg.Any<EmployerNameDto>());
        }
    }
}