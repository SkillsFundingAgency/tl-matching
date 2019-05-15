using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Domain.EqualityComparer
{
    public sealed class ProviderEqualityComparer : IEqualityComparer<Provider>
    {
        public bool Equals(Provider x, Provider y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.UkPrn == y.UkPrn && string.Equals(x.Name, y.Name) && x.OfstedRating == y.OfstedRating && string.Equals(x.PrimaryContact, y.PrimaryContact) && string.Equals(x.PrimaryContactEmail, y.PrimaryContactEmail) && string.Equals(x.PrimaryContactPhone, y.PrimaryContactPhone) && string.Equals(x.SecondaryContact, y.SecondaryContact) && string.Equals(x.SecondaryContactEmail, y.SecondaryContactEmail) && string.Equals(x.SecondaryContactPhone, y.SecondaryContactPhone) && string.Equals(x.Source, y.Source) && Equals(x.ProviderVenue, y.ProviderVenue);
        }

        public int GetHashCode(Provider obj)
        {
            unchecked
            {
                var hashCode = obj.UkPrn.GetHashCode();
                hashCode = (hashCode * 397) ^ (obj.Name != null ? obj.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ obj.OfstedRating;
                hashCode = (hashCode * 397) ^ (obj.PrimaryContact != null ? obj.PrimaryContact.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.PrimaryContactEmail != null ? obj.PrimaryContactEmail.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.PrimaryContactPhone != null ? obj.PrimaryContactPhone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.SecondaryContact != null ? obj.SecondaryContact.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.SecondaryContactEmail != null ? obj.SecondaryContactEmail.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.SecondaryContactPhone != null ? obj.SecondaryContactPhone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Source != null ? obj.Source.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.ProviderVenue != null ? obj.ProviderVenue.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}