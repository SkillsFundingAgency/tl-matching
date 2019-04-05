using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Domain.EqualityComparer
{
    public sealed class ProviderVenueEqualityComparer : IEqualityComparer<ProviderVenue>
    {
        public bool Equals(ProviderVenue x, ProviderVenue y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.ProviderId == y.ProviderId && string.Equals(x.Town, y.Town) && string.Equals(x.County, y.County) && string.Equals(x.Postcode, y.Postcode) && x.Latitude == y.Latitude && x.Longitude == y.Longitude && string.Equals(x.Source, y.Source) && Equals(x.Provider, y.Provider) && Equals(x.ProviderQualification, y.ProviderQualification) && Equals(x.Referral, y.Referral) && Equals(x.Location, y.Location);
        }

        public int GetHashCode(ProviderVenue obj)
        {
            unchecked
            {
                var hashCode = obj.ProviderId;
                hashCode = (hashCode * 397) ^ (obj.Town != null ? obj.Town.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.County != null ? obj.County.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Postcode != null ? obj.Postcode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ obj.Latitude.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Longitude.GetHashCode();
                hashCode = (hashCode * 397) ^ (obj.Source != null ? obj.Source.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Provider != null ? obj.Provider.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.ProviderQualification != null ? obj.ProviderQualification.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Referral != null ? obj.Referral.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Location != null ? obj.Location.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}