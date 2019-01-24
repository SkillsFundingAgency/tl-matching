// TODO AU ADD BACK IN
//using System;
//using System.IO;
//using NUnit.Framework;

//namespace Sfa.Tl.Matching.FileReader.Csv.IntegrationTests.Employer
//{
//    public class Csv_Loaded_Successfully
//    {
//        private EmployerLoadResult _loadResult;

//        private const string DataFilePath = "./Employers/Employer-Simple.csv";
//        private Employer _firstRecord;

//        [SetUp]
//        public void Setup()
//        {
//            var fileReader = new CsvEmployerFileReader();
//            using (var stream = File.Open(DataFilePath, FileMode.Open))
//            {
//                _loadResult = fileReader.Load(stream);
//                _firstRecord = _loadResult.Data[0];
//            }
//        }

//        [Fact(DisplayName = "Correct number of records loaded")]
//        public void CorrectNumberOfRecordsLoaded() =>
//            Assert.Single(_loadResult.Data);


//        #region 1st Record Tests
//        [Fact(DisplayName = "1st record - Account is correct")]
//        public void ContactIsCorrectForRecord1() =>
//            Assert.Equal(new Guid("9082609f-9cf8-e811-80e0-000d3a214f60"), _firstRecord.Account);

//        [Fact(DisplayName = "1st record - Checksum is correct")]
//        public void ChecksumIsCorrectForRecord1() =>
//            Assert.Equal("Checksum", _firstRecord.Checksum);

//        [Fact(DisplayName = "1st record - Modified On is correct")]
//        public void ModifiedOnIsCorrectForRecord1() =>
//            Assert.Equal(new DateTime(2018, 12, 5, 14, 53, 48), _firstRecord.ModifiedOn);

//        [Fact(DisplayName = "1st record - Company Name is correct")]
//        public void CompanyNameIsCorrectForRecord1() =>
//            Assert.Equal("Company Name", _firstRecord.CompanyName);

//        [Fact(DisplayName = "1st record - Company Aka is correct")]
//        public void CompanyAkaIsCorrectForRecord1() =>
//            Assert.Equal("Also Known As", _firstRecord.CompanyAka);

//        [Fact(DisplayName = "1st record - Aupa is correct")]
//        public void AupaIsCorrectForRecord1() =>
//            Assert.Equal("Aware", _firstRecord.Aupa);

//        [Fact(DisplayName = "1st record - Company Type is correct")]
//        public void CompanyTypeIsCorrectForRecord1() =>
//            Assert.Equal("Employer", _firstRecord.CompanyType);

//        [Fact(DisplayName = "1st record - Primary Contact is correct")]
//        public void PrimaryContactIsCorrectForRecord1() =>
//            Assert.Equal("Primary Contact", _firstRecord.PrimaryContact);

//        [Fact(DisplayName = "1st record - Phone is correct")]
//        public void PhoneIsCorrectForRecord1() =>
//            Assert.Equal("7777744465", _firstRecord.Phone);

//        [Fact(DisplayName = "1st record - Email is correct")]
//        public void EmailIsCorrectForRecord1() =>
//            Assert.Equal("email@address.com", _firstRecord.Email);

//        [Fact(DisplayName = "1st record - Website is correct")]
//        public void WebsiteIsCorrectForRecord1() =>
//            Assert.Equal("www.website.com", _firstRecord.Website);

//        [Fact(DisplayName = "1st record - Address 1 is correct")]
//        public void Address1IsCorrectForRecord1() =>
//            Assert.Equal("Address1", _firstRecord.Address1);

//        [Fact(DisplayName = "1st record - City is correct")]
//        public void CityIsCorrectForRecord1() =>
//            Assert.Equal("City", _firstRecord.City);

//        [Fact(DisplayName = "1st record - Country Region is correct")]
//        public void CountryRegionIsCorrectForRecord1() =>
//            Assert.Equal("Region", _firstRecord.CountryRegion);

//        [Fact(DisplayName = "1st record - PostCode is correct")]
//        public void PostCodeIsCorrectForRecord1() =>
//            Assert.Equal("S1 1AA", _firstRecord.PostCode);

//        [Fact(DisplayName = "1st record - Created By is correct")]
//        public void CreatedByIsCorrectForRecord1() =>
//            Assert.Equal("Created By", _firstRecord.CreatedBy);

//        [Fact(DisplayName = "1st record - Created is correct")]
//        public void CreatedIsCorrectForRecord1() =>
//            Assert.Equal(new DateTime(2018, 12, 5, 14, 47, 1), _firstRecord.Created);

//        [Fact(DisplayName = "1st record - Modified By is correct")]
//        public void ModifiedByIsCorrectForRecord1() =>
//            Assert.Equal("Modified By", _firstRecord.ModifiedBy);

//        [Fact(DisplayName = "1st record - Modified is correct")]
//        public void ModifiedIsCorrectForRecord1() =>
//            Assert.Equal(new DateTime(2018, 12, 5, 14, 53, 48), _firstRecord.Modified);

//        [Fact(DisplayName = "1st record - Owner is correct")]
//        public void OwnerIsCorrectForRecord1() =>
//            Assert.Equal("Owner", _firstRecord.Owner);
//        #endregion
//    }
//}