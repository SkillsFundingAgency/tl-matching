using System.Collections.Generic;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders
{
    internal class OpportunityBasketViewModelBuilder
    {
        private readonly OpportunityBasketViewModel _viewModel;

        public OpportunityBasketViewModelBuilder()
        {
            _viewModel = new OpportunityBasketViewModel
            {
                CompanyName = "CompanyName",
                CompanyNameAka = "AlsoKnownAs",
                ReferralItems = new List<BasketReferralItemViewModel>(),
                ProvisionGapItems = new List<BasketProvisionGapItemViewModel>()
            };
        }

        internal OpportunityBasketViewModelBuilder AddReferralItem()
        {
            _viewModel.ReferralItems.Add(new BasketReferralItemViewModel
            {
                Providers = 1,
                JobRole = "Referral"
            });

            return this;
        }

        internal OpportunityBasketViewModelBuilder AddProvisionGapItem()
        {
            _viewModel.ProvisionGapItems.Add(new BasketProvisionGapItemViewModel
            {
                JobRole = "ProvisionGap",
                Reason = "Reason"
            });

            return this;
        }

        public OpportunityBasketViewModel Build() => _viewModel;
    }
}
