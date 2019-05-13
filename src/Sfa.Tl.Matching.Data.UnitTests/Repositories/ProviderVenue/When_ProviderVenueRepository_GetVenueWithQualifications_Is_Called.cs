using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderVenue
{
    public class When_ProviderVenueRepository_GetVenueWithQualifications_Is_Called
    {
        private readonly ProviderVenueDetailViewModel _result;

        private const int Id = 1;
        private const int ProviderId = 1;
        private const string Postcode = "AA1 1AA";
        private const string ProviderName = "ProviderName";
        private const string VenueName = "VenueName";
        private const bool IsEnabledForSearchProvider = true;
        private const bool IsEnabledForSearchProviderVenue = false;
        private const bool IsRemoved = false;
        private const string LarsId = "1234";
        private const string Title = "Title";
        private const string ShortTitle = "ShortTitle";

        public When_ProviderVenueRepository_GetVenueWithQualifications_Is_Called()
        {
            var logger = Substitute.For<ILogger<ProviderVenueRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new Domain.Models.ProviderVenue
                {
                    Id = Id,
                    Name = VenueName,
                    Postcode = Postcode,
                    IsEnabledForReferral = IsEnabledForSearchProviderVenue,
                    IsRemoved = IsRemoved,
                    Provider = new Domain.Models.Provider
                    {
                        Id = ProviderId,
                        Name = ProviderName,
                        IsCdfProvider = IsEnabledForSearchProvider
                    },
                    ProviderQualification = new List<Domain.Models.ProviderQualification>
                    {
                        new Domain.Models.ProviderQualification
                        {
                            Qualification = new Domain.Models.Qualification
                            {
                                LarsId = LarsId,
                                Title = Title,
                                ShortTitle = ShortTitle
                            }
                        }
                    }
                });
                dbContext.SaveChanges();

                var repository = new ProviderVenueRepository(logger, dbContext);
                _result = repository.GetVenueWithQualifications(Id)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Id_Is_Returned() =>
            _result.Id.Should().Be(Id);

        [Fact]
        public void Then_ProviderId_Is_Returned() =>
            _result.ProviderId.Should().Be(ProviderId);

        [Fact]
        public void Then_ProviderName_Is_Returned() =>
            _result.ProviderName.Should().Be(ProviderName);

        [Fact]
        public void Then_Postcode_Is_Returned() =>
            _result.Postcode.Should().BeEquivalentTo(Postcode);

        [Fact]
        public void Then_VenueName_Is_Returned() =>
            _result.Name.Should().Be(VenueName);

        [Fact]
        public void Then_IsEnabledForSearchProviderVenue_Is_Returned() =>
            _result.IsRemoved.Should().Be(IsRemoved);

        [Fact]
        public void Then_First_Qualification_LarsId_Is_Returned() =>
            _result.Qualifications[0].LarsId.Should().Be(LarsId);

        [Fact]
        public void Then_First_Qualification_Title_Is_Returned() =>
            _result.Qualifications[0].Title.Should().Be(Title);

        [Fact]
        public void Then_First_Qualification_ShortTitle_Is_Returned() =>
            _result.Qualifications[0].ShortTitle.Should().Be(ShortTitle);
    }
}