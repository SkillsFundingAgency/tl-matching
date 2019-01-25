using System;
using Sfa.Tl.Matching.Domain.Enums;
using Sfa.Tl.Matching.FileReader.Excel.Employer;

namespace Sfa.Tl.Matching.Application.Employer
{
    public class EmployerReaderFactory
    {
        public static IEmployerFileReader Create(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Excel:
                    return new ExcelEmployerFileReader();
            }

            throw new InvalidOperationException();
        }
    }
}