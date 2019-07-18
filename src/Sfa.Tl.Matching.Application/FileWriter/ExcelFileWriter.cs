using System.IO;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Application.FileWriter
{
    public class ExcelFileWriter<TDto> : IFileWriter<TDto> where TDto : class, new()
    {
        public ExcelFileWriter()
        {

        }

        public async Task<Stream> WriteReport(TDto data)
        {
            //Takes dto for report?

            //Create spreadsheet in stream
            var stream = new MemoryStream();
            
            //returns stream? Would need to rely on caller to dispose it
            return stream;
        }
    }
}
