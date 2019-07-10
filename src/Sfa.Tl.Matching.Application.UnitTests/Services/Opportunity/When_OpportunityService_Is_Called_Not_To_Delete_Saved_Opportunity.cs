using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using NSubstitute;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.AutoDomain;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
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
            List<OpportunityItem> opportunityItems,
            List<Domain.Models.Referral> referralItems,
            List<ProvisionGap> provisionGapItems,
            OpportunityService sut)
        {
            //Arrange
            opportunityItems = opportunityItems.Where(x => x.IsSaved).Select(opportunityItem =>
            {
                opportunityItem.IsSaved = true;
                return opportunityItem;
            }).ToList();

            var item = opportunityItems.First();

            opportunityItemRepo.GetMany(Arg.Any<Expression<Func<OpportunityItem, bool>>>()).Returns(opportunityItems.AsQueryable());
            referralRepo.GetMany(Arg.Any<Expression<Func<Domain.Models.Referral, bool>>>()).Returns(referralItems.AsQueryable());
            provisionGapRepo.GetMany(Arg.Any<Expression<Func<ProvisionGap, bool>>>()).Returns(provisionGapItems.AsQueryable());

            //Act
            await sut.DeleteOpportunityItemAsync(item.OpportunityId, item.OpportunityId);

            //Assert
            await opportunityRepo.DidNotReceive().Delete(Arg.Any<int>());

        }

        [Theory, AutoDomainData]
        public async Task Then_Do_Not_Delete_Opportunity_Item([Frozen] IOpportunityRepository opportunityRepo,
            [Frozen] IRepository<OpportunityItem> opportunityItemRepo,
            [Frozen] IRepository<Domain.Models.Referral> referralRepo,
            [Frozen] IRepository<ProvisionGap> provisionGapRepo,
            List<OpportunityItem> opportunityItems,
            List<Domain.Models.Referral> referralItems,
            List<ProvisionGap> provisionGapItems,
            OpportunityService sut)
        {
            //Arrange
            opportunityItems = opportunityItems.Where(x => x.IsSaved).Select(opportunityItem =>
            {
                opportunityItem.IsSaved = true;
                return opportunityItem;
            }).ToList();

            var item = opportunityItems.First();

            opportunityItemRepo.GetMany(Arg.Any<Expression<Func<OpportunityItem, bool>>>()).Returns(opportunityItems.AsQueryable());
            referralRepo.GetMany(Arg.Any<Expression<Func<Domain.Models.Referral, bool>>>()).Returns(referralItems.AsQueryable());
            provisionGapRepo.GetMany(Arg.Any<Expression<Func<ProvisionGap, bool>>>()).Returns(provisionGapItems.AsQueryable());

            //Act
            await sut.DeleteOpportunityItemAsync(item.OpportunityId, item.OpportunityId);

            //Assert
            await opportunityRepo.DidNotReceive().Delete(Arg.Any<int>());
        }

        [Theory, AutoDomainData]
        public async Task Then_Delete_Referral_Is_Called_Once(
            [Frozen] IRepository<OpportunityItem> opportunityItemRepo,
            [Frozen] IRepository<Domain.Models.Referral> referralRepo,
            [Frozen] IRepository<ProvisionGap> provisionGapRepo,
            List<OpportunityItem> opportunityItems,
            List<Domain.Models.Referral> referralItems,
            List<ProvisionGap> provisionGapItems,
            OpportunityService sut)
        {
            //Arrange
            opportunityItems = opportunityItems.Where(x => x.IsSaved).Select(opportunityItem =>
            {
                opportunityItem.IsSaved = true;
                return opportunityItem;
            }).ToList();

            var item = opportunityItems.First();

            opportunityItemRepo.GetMany(Arg.Any<Expression<Func<OpportunityItem, bool>>>()).Returns(opportunityItems.AsQueryable());
            referralRepo.GetMany(Arg.Any<Expression<Func<Domain.Models.Referral, bool>>>()).Returns(referralItems.AsQueryable());
            provisionGapRepo.GetMany(Arg.Any<Expression<Func<ProvisionGap, bool>>>()).Returns(provisionGapItems.AsQueryable());

            //Act
            await sut.DeleteOpportunityItemAsync(item.OpportunityId, item.OpportunityId);

            //Assert
            await referralRepo.Received(1).DeleteMany(Arg.Any<List<Domain.Models.Referral>>());
        }

        [Theory, AutoDomainData]
        public async Task Then_Delete_Provision_Gap_Is_Called_Once(
            [Frozen] IRepository<OpportunityItem> opportunityItemRepo,
            [Frozen] IRepository<Domain.Models.Referral> referralRepo,
            [Frozen] IRepository<ProvisionGap> provisionGapRepo,
            List<OpportunityItem> opportunityItems,
            List<Domain.Models.Referral> referralItems,
            List<ProvisionGap> provisionGapItems,
            OpportunityService sut)
        {
            //Arrange
            opportunityItems = opportunityItems.Where(x => x.IsSaved).Select(opportunityItem =>
            {
                opportunityItem.IsSaved = true;
                return opportunityItem;
            }).ToList();

            var item = opportunityItems.First();

            opportunityItemRepo.GetMany(Arg.Any<Expression<Func<OpportunityItem, bool>>>()).Returns(opportunityItems.AsQueryable());
            referralRepo.GetMany(Arg.Any<Expression<Func<Domain.Models.Referral, bool>>>()).Returns(referralItems.AsQueryable());
            provisionGapRepo.GetMany(Arg.Any<Expression<Func<ProvisionGap, bool>>>()).Returns(provisionGapItems.AsQueryable());

            //Act
            await sut.DeleteOpportunityItemAsync(item.OpportunityId, item.OpportunityId);
            
            //Assert
            await provisionGapRepo.Received(1).DeleteMany(Arg.Any<List<ProvisionGap>>());
        }
    }
}