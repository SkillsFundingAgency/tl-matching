using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Domain.EqualityComparer
{
    public sealed class EmployerStagingEqualityComparer : IEqualityComparer<EmployerStaging>
    {
        public bool Equals(EmployerStaging x, EmployerStaging y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.CrmId.Equals(y.CrmId) && string.Equals(x.CompanyName, y.CompanyName) && string.Equals(x.AlsoKnownAs, y.AlsoKnownAs) && string.Equals(x.Aupa, y.Aupa) && string.Equals(x.CompanyType, y.CompanyType) && string.Equals(x.PrimaryContact, y.PrimaryContact) && string.Equals(x.Phone, y.Phone) && string.Equals(x.Email, y.Email) && string.Equals(x.Postcode, y.Postcode) && string.Equals(x.Owner, y.Owner);
        }

        public int GetHashCode(EmployerStaging obj)
        {
            unchecked
            {
                var hashCode = obj.CrmId.GetHashCode();
                hashCode = (hashCode * 397) ^ (obj.CompanyName != null ? obj.CompanyName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.AlsoKnownAs != null ? obj.AlsoKnownAs.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Aupa != null ? obj.Aupa.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.CompanyType != null ? obj.CompanyType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.PrimaryContact != null ? obj.PrimaryContact.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Phone != null ? obj.Phone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Email != null ? obj.Email.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Postcode != null ? obj.Postcode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Owner != null ? obj.Owner.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}