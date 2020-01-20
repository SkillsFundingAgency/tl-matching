using System;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Web.Tests.Common.Database.StandingData
{
    internal class EmployerData
    {
        internal static Employer[] Create()
        {
            var employers = new[]
            {
                new Employer
                {
                    //Id = 1,
                    CrmId = new Guid("11111111-1111-1111-1111-111111111111"),
                    CompanyName = "Company Name",
                    AlsoKnownAs = "",
                    CompanyNameSearch = "CompanyName",
                    Aupa = "Active",
                    PrimaryContact = "Primary Contact",
                    Phone = "07878 787558",
                    Email = "email@address.com",
                    Owner = "Owner",
                    CreatedOn = new DateTime(2019, 1, 1),
                    CreatedBy = "Dev Surname"
                },
                new Employer
                {
                    //Id = 2,
                    CrmId = new Guid("22222222-2222-2222-2222-222222222222"),
                    CompanyName = "Company Name For Selection",
                    AlsoKnownAs = "",
                    CompanyNameSearch = "CompanyNameForSelection",
                    Aupa = "Active",
                    PrimaryContact = "Primary Contact",
                    Phone = "07878 787558",
                    Email = "email@address.com",
                    Owner = "Owner",
                    CreatedOn = new DateTime(2019, 1, 1),
                    CreatedBy = "Dev Surname"
                }
            };

            return employers;
        }
    }
}