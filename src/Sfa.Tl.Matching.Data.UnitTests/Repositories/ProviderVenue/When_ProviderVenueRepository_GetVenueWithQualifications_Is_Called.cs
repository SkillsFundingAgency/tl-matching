using System.Collections.Generic;
using System.Linq;
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
        private const bool IsCdfProvider = true;
        private const bool IsEnabledForReferral = false;
        private const bool IsRemoved = false;
        private const int QualificationId = 99;
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
                    IsEnabledForReferral = IsEnabledForReferral,
                    IsRemoved = IsRemoved,
                    Provider = new Domain.Models.Provider
                    {
                        Id = ProviderId,
                        Name = ProviderName,
                        IsCdfProvider = IsCdfProvider
                    },
                    ProviderQualification = new List<Domain.Models.ProviderQualification>
                    {
                        new Domain.Models.ProviderQualification
                        {
                            Qualification = new Domain.Models.Qualification
                            {
                                Id = QualificationId,
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
        public void Then_Result_Fields_Have_Expected_Value()
        {
            _result.Id.Should().Be(Id);
            _result.ProviderId.Should().Be(ProviderId);
            _result.ProviderName.Should().Be(ProviderName);
            _result.Postcode.Should().BeEquivalentTo(Postcode);
            _result.Name.Should().Be(VenueName);
        }

        [Fact]
        public void Then_First_Qualification_Fields_Have_Expected_Values()
        {
            var qualification = _result.Qualifications.First();
            qualification.Id.Should().Be(QualificationId);
            qualification.LarsId.Should().Be(LarsId);
            qualification.Title.Should().Be(Title);
            qualification.ShortTitle.Should().Be(ShortTitle);
        }
    }
}