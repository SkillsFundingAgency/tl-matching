
namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IFileWriter<in TDto> where TDto : class, new()
    {
        byte[] WriteReport(TDto data);
    }
}