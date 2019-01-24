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
            var fileLoadResult = new EmployerLoadResult();
            var employers = new List<Employer>();

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
                    var fileUploadEmployer = CreateEmployer(document, row);
                    employers.Add(fileUploadEmployer);
                }

                fileLoadResult.Data = employers;

                return fileLoadResult;
            }
        }

        #region Private Methods
        private static Employer CreateEmployer(SpreadsheetDocument document, OpenXmlElement row)
        {
            var account = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Account));
            var companyName = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.CompanyName));
            var companyAka = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.CompanyAka));
            var aupa = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Aupa));
            var companyType = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.CompanyType));
            var phone = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Phone));
            var email = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Email));
            var website = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Website));
            // TODO AU Where is this? var addressName
            var address1 = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Address1));
            // TODO AU Where is this? var address1
            // TODO AU Where is this? var address2
            // TODO AU Where is this? var address3
            var city = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.City));
            // TODO AU Where is this? var county
            var postCode = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.PostCode));
            var createdBy = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.CreatedBy));
            var created = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Created));
            var modifiedBy = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.ModifiedBy));
            var modified = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Modified));
            var owner = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.Owner));
            
            //var modifiedOn = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.ModifiedOn));
            //var primaryContact = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.PrimaryContact));
            //var countryRegion = CellValueRetriever.Get(document, row.Descendants<Cell>().ElementAt(EmployerColumnIndex.CountryRegion));

            var employer = new Employer
            {
                Account = new Guid(account),
                CompanyName = companyName,
                AlsoKnownAs = companyAka,
                // TODO AU ADD BACK IN AupaStatus = aupa,
                // TODO AU ADD BACK IN CompanyType = companyType,
                Phone = phone,
                Email = email,
                Website = website,
                Address1 = address1,
                City = city,
                PostCode = postCode,
                CreatedBy = createdBy,
                CreatedOn = created.ToDate(),
                ModifiedBy = modifiedBy,
                ModifiedOn = modified.ToDate(),
                Owner = owner
            };

            return employer;
        }
        #endregion
    }
}