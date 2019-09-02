using System;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Web.Tests.Common.Database.StandingData
{
    internal class MaintenanceHistoryData
    {
        internal static MaintenanceHistory[] Create()
        {
            var maintenanceHistories = new[]
            {
                new MaintenanceHistory
                {
                    Id = 1,
                    IsOnline = false,
                    CreatedOn = new DateTime(2019, 1, 1),
                    CreatedBy = "Dev Surname"
                },
                new MaintenanceHistory
                {
                    Id = 2,
                    IsOnline = true,
                    CreatedOn = new DateTime(2019, 1, 2),
                    CreatedBy = "Dev Surname"
                }
            };

            return maintenanceHistories;
        }
    }
}