using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Get_Latest_Completed
    {
        private readonly OpportunityDto _opportunityDto;
        private readonly IRepository<Domain.Models.Opportunity> _opportunityRepository;

        private readonly Guid _crmId = new Guid("65021261-8C70-4C4F-954F-4E5282250A85");

        public When_OpportunityService_Is_Called_To_Get_Latest_Completed()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);

            _opportunityRepository = Substitute.For<IRepository<Domain.Models.Opportunity>>();

            _opportunityRepository.GetMany(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>())
                .Returns(new List<Domain.Models.Opportunity>
                {
                    new Domain.Models.Opportunity
                    {
                        Id = 1,
                        EmployerCrmId = _crmId,
                        CreatedOn = new DateTime(2019, 1, 1, 23, 59, 58),
                        ProvisionGap = new List<ProvisionGap>
                        {
                            new ProvisionGap
                            {
                                OpportunityId = 1
                            }
                        }
                    },
                    new Domain.Models.Opportunity
                    {
                        Id = 2,
                        CreatedOn = new DateTime(2019, 1, 1, 23, 59, 59),
                        ProvisionGap = new List<ProvisionGap>
                        {
                            new ProvisionGap
                            {
                                OpportunityId = 2
                            }
                        }
                    },
                    new Domain.Models.Opportunity
                    {
                        Id = 3,
                        CreatedOn = new DateTime(2018, 1, 1, 23, 59, 59),
                        ConfirmationSelected = true,
                        Referral = new List<Domain.Models.Referral>
                        {
                            new Domain.Models.Referral
                            {
                                OpportunityId = 3
                            }
                        }
                    }
                }.AsQueryable());

            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            var opportunityService = new OpportunityService(mapper, _opportunityRepository, provisionGapRepository,
                referralRepository);

            _opportunityDto = opportunityService.GetLatestCompletedOpportunity(_crmId);
        }

        [Fact]
        public void Then_GetMany_Is_Called_Exactly_Once()
        {
            _opportunityRepository.GetMany(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>());
        }

        [Fact]
        public void Then_Correct_Opportunity_Is_Returned()
        {
            _opportunityDto.Id.Should().Be(2);
        }
    }
}