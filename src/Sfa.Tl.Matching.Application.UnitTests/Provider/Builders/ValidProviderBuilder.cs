namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Builders
{
    internal class ValidProviderBuilder
    {
        public static Domain.Models.Provider Build() => new Domain.Models.Provider
        {
            Id = 1,
            UkPrn = 10000546,
        };
    }
}