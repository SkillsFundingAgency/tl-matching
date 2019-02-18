using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Route.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Route.Builders
{
    internal class ValidRouteListBuilder
    {
        private readonly IList<Domain.Models.Route> _routes;

        public ValidRouteListBuilder()
        {
            _routes =
                new List<Domain.Models.Route>
                {
                    new Domain.Models.Route
                    {
                        Id = RouteConstants.Id,
                        Name = RouteConstants.Name,
                        Keywords = RouteConstants.Keywords,
                        Summary = RouteConstants.Summary,
                        CreatedBy = EntityCreationConstants.CreatedByUser,
                        CreatedOn = EntityCreationConstants.CreatedOn,
                        ModifiedBy = EntityCreationConstants.ModifiedByUser,
                        ModifiedOn = EntityCreationConstants.ModifiedOn
                    },
                    new Domain.Models.Route
                    {
                        Id = RouteConstants.Id + 1,
                        Name = RouteConstants.SecondName,
                        Keywords = RouteConstants.Keywords,
                        Summary = RouteConstants.Summary,
                        CreatedBy = EntityCreationConstants.CreatedByUser,
                        CreatedOn = EntityCreationConstants.CreatedOn,
                        ModifiedBy = EntityCreationConstants.ModifiedByUser,
                        ModifiedOn = EntityCreationConstants.ModifiedOn
                    }
                };
        }

        public IEnumerable<Domain.Models.Route> Build() =>
            _routes;
    }
}
