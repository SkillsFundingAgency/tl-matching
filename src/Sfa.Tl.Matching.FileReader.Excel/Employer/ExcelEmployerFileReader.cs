using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Sfa.Tl.Matching.FileReader.Excel.Employer
{
    public class ExcelEmployerFileReader : IEmployerFileReader
    {
        public EmployerLoadResult Load(Stream stream)
        {
            var fileEmployers = new List<FileEmployer>();

            using (var document = SpreadsheetDocument.Open(stream, false))
            {
                var workbookPart = document.WorkbookPart;
                var sheets = workbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                var relationshipId = sheets.First().Id.Value;
                var worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(relationshipId);
                var workSheet = worksheetPart.Worksheet;
                var sheetData = workSheet.GetFirstChild<SheetData>();
                var rows = sheetData.Descendants<Row>().ToList();
                rows.RemoveAt(0);

                foreach (var row in rows)
                {
                    var fileEmployer = CreateEmployer(document, row);
                    fileEmployers.Add(fileEmployer);
                }

                var fileLoadResult = new EmployerLoadResult(fileEmployers, string.Empty);

                return fileLoadResult;
            }
        }

        #region Private Methods
        private static FileEmployer CreateEmployer(SpreadsheetDocument document, OpenXmlElement row)
        {
            var crmId = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt((int)ExcelEmployerColumn.Account));
            var companyName = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt((int)ExcelEmployerColumn.CompanyName));
            var companyAka = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt((int)ExcelEmployerColumn.CompanyAka));
            var primaryContact = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt((int)ExcelEmployerColumn.PrimaryContact));
            var phone = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt((int)ExcelEmployerColumn.Phone));
            var email = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt((int)ExcelEmployerColumn.Email));
            var postCode = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt((int)ExcelEmployerColumn.PostCode));
            var owner = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt((int)ExcelEmployerColumn.Owner));

            var fileEmployer = new FileEmployer
            {
                CrmId = new Guid(crmId),
                CompanyName = companyName,
                AlsoKnownAs = companyAka,
                PrimaryContact = primaryContact,
                Phone = phone,
                Email = email,
                PostCode = postCode,
                Owner = owner
            };

            return fileEmployer;
        }
        #endregion
    }
}