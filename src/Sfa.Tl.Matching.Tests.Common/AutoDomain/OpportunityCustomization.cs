using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using Humanizer;

namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public class OpportunityCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new PropertyNameOmitter("ProviderVenue", "Location", "Id"));
            fixture.Customizations.Add(new PropertyMaxLengthSetter(new Dictionary<string, int>
            {
                { "TemplateName", 50 },
                { "TemplateId", 50 },
                { "Status", 10 },
                { "Aupa", 10 },
                { "LarId", 8 },
            }));
        }
    }

    internal class PropertyNameOmitter : ISpecimenBuilder
    {
        private readonly IEnumerable<string> _names;

        internal PropertyNameOmitter(params string[] names)
        {
            _names = names;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var propInfo = request as PropertyInfo;
            if (propInfo != null && _names.Contains(propInfo.Name))
                return new OmitSpecimen();

            return new NoSpecimen();
        }
    }

    internal class PropertyMaxLengthSetter : ISpecimenBuilder
    {
        private readonly IDictionary<string, int> _names;

        internal PropertyMaxLengthSetter(IDictionary<string, int> names)
        {
            _names = names;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var propInfo = request as PropertyInfo;

            if (propInfo != null)
            {
                var sholdTruncate = _names.TryGetValue(propInfo.Name, out var length);

                if (sholdTruncate && propInfo.PropertyType == typeof(string))
                {
                    var rsult =  context.Create<string>().Truncate(length);
                    return rsult;
                }

            }

            return new NoSpecimen();
        }
    }
}