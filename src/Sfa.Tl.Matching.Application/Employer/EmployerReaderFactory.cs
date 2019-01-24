using System;
using Sfa.Tl.Matching.Core.Enums;
using Sfa.Tl.Matching.FileReader.Excel.Employer;

namespace Sfa.Tl.Matching.Application.Employer
{
    public class EmployerReaderFactory
    {
        public static IEmployerFileReader Create(FileType fileType)
        {
            switch (fileType)
            {
                // TODO AU ADD BACK IN
                //case FileType.Csv:
                //    return new CsvEmployerFileReader();
                case FileType.Excel:
                    return new ExcelEmployerFileReader();
            }

            throw new InvalidOperationException();
        }
    }
}