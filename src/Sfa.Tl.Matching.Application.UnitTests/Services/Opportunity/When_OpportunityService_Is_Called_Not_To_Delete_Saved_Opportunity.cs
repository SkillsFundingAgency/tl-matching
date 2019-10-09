using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using NSubstitute;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_Not_To_Delete_Saved_Opportunity
    {
        [Theory, AutoDomainData]
        public async Task Then_Do_Not_Delete_Opportunity(
            [Frozen] IOpportunityRepository opportunityRepo,
            [Frozen] IRepository<OpportunityItem> opportunityItemRepo,
            [Frozen] IRepository<Domain.Models.Referral> referralRepo,
            [Frozen] IRepository<ProvisionGap> provisionGapRepo,
            OpportunityItem opportunityItem,
            OpportunityService sut)
        {
            //Arrange
            opportunityItem.IsSaved = true;

            opportunityItemRepo.GetManyAsync(Arg.Any<Expression<Func<OpportunityItem, bool>>>()).Returns(new List<OpportunityItem>
            {
                opportunityItem
            }.AsQueryable());

            referralRepo.GetManyAsync(Arg.Any<Expression<Func<Domain.Models.Referral, bool>>>()).Returns(opportunityItem.Referral.AsQueryable());
            provisionGapRepo.GetManyAsync(Arg.Any<Expression<Func<ProvisionGap, bool>>>()).Returns(opportunityItem.ProvisionGap.AsQueryable());

            //Act
            await sut.DeleteOpportunityItemAsync(opportunityItem.OpportunityId, opportunityItem.OpportunityId);

            //Assert
            await opportunityRepo.DidNotReceive().DeleteAsync(Arg.Any<int>());

        }

        [Theory, AutoDomainData]
        public async Task Then_Do_Not_Delete_Opportunity_Item([Frozen] IOpportunityRepository opportunityRepo,
            [Frozen] IRepository<OpportunityItem> opportunityItemRepo,
            [Frozen] IRepository<Domain.Models.Referral> referralRepo,
            [Frozen] IRepository<ProvisionGap> provisionGapRepo,
            OpportunityItem opportunityItem,
            OpportunityService sut)
        {
            //Arrange
            opportunityItem.IsSaved = true;

            opportunityItemRepo.GetManyAsync(Arg.Any<Expression<Func<OpportunityItem, bool>>>()).Returns(new List<OpportunityItem>
            {
                opportunityItem
            }.AsQueryable());

            referralRepo.GetManyAsync(Arg.Any<Expression<Func<Domain.Models.Referral, bool>>>()).Returns(opportunityItem.Referral.AsQueryable());
            provisionGapRepo.GetManyAsync(Arg.Any<Expression<Func<ProvisionGap, bool>>>()).Returns(opportunityItem.ProvisionGap.AsQueryable());

            //Act
            await sut.DeleteOpportunityItemAsync(opportunityItem.OpportunityId, opportunityItem.OpportunityId);

            //Assert
            await opportunityRepo.DidNotReceive().DeleteAsync(Arg.Any<int>());
        }

        [Theory, AutoDomainData]
        public async Task Then_Delete_Referral_Is_Called_Once(
            [Frozen] IRepository<OpportunityItem> opportunityItemRepo,
            [Frozen] IRepository<Domain.Models.Referral> referralRepo,
            [Frozen] IRepository<ProvisionGap> provisionGapRepo,
            OpportunityItem opportunityItem,
            OpportunityService sut)
        {
            //Arrange
            opportunityItem.IsSaved = true;

            opportunityItemRepo.GetManyAsync(Arg.Any<Expression<Func<OpportunityItem, bool>>>()).Returns(new List<OpportunityItem>
            {
                opportunityItem
            }.AsQueryable());

            referralRepo.GetManyAsync(Arg.Any<Expression<Func<Domain.Models.Referral, bool>>>()).Returns(opportunityItem.Referral.AsQueryable());
            provisionGapRepo.GetManyAsync(Arg.Any<Expression<Func<ProvisionGap, bool>>>()).Returns(opportunityItem.ProvisionGap.AsQueryable());

            //Act
            await sut.DeleteOpportunityItemAsync(opportunityItem.OpportunityId, opportunityItem.OpportunityId);

            //Assert
            await referralRepo.Received(1).DeleteManyAsync(Arg.Any<List<Domain.Models.Referral>>());
        }

        [Theory, AutoDomainData]
        public async Task Then_Delete_Provision_Gap_Is_Called_Once(
            [Frozen] IRepository<OpportunityItem> opportunityItemRepo,
            [Frozen] IRepository<Domain.Models.Referral> referralRepo,
            [Frozen] IRepository<ProvisionGap> provisionGapRepo,
            OpportunityItem opportunityItem,
            OpportunityService sut)
        {
            //Arrange
            opportunityItem.IsSaved = true;

            opportunityItemRepo.GetManyAsync(Arg.Any<Expression<Func<OpportunityItem, bool>>>()).Returns(new List<OpportunityItem>
            {
                opportunityItem
            }.AsQueryable());

            referralRepo.GetManyAsync(Arg.Any<Expression<Func<Domain.Models.Referral, bool>>>()).Returns(opportunityItem.Referral.AsQueryable());
            provisionGapRepo.GetManyAsync(Arg.Any<Expression<Func<ProvisionGap, bool>>>()).Returns(opportunityItem.ProvisionGap.AsQueryable());

            //Act
            await sut.DeleteOpportunityItemAsync(opportunityItem.OpportunityId, opportunityItem.OpportunityId);
            
            //Assert
            await provisionGapRepo.Received(1).DeleteManyAsync(Arg.Any<List<ProvisionGap>>());
        }
    }
}