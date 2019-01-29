namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderService
    {
        void ImportProvider();
        void UpdateProvider();
        void SearchProviderByPostCodeProximity();
    }
}