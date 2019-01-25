using System;
using System.IO;
using NUnit.Framework;
using Sfa.Tl.Matching.FileReader.Excel.Employer;

namespace Sfa.Tl.Matching.FileReader.Excel.IntegrationTests.Employer
{
    public class When_ExcelEmployerFileReader_Load_Is_Called
    {
        private EmployerLoadResult _loadResult;

        private const string DataFilePath = "./Employer/Employer-Simple.xlsx";
        private FileEmployer _firstRecord;

        [SetUp]
        public void Setup()
        {
            var fileReader = new ExcelEmployerFileReader();
            using (var stream = File.Open(DataFilePath, FileMode.Open))
            {
                _loadResult = fileReader.Load(stream);
                _firstRecord = _loadResult.Data[0];
            }
        }

        [Test]
        public void Then_Count_Of_Records_Is_1() =>
            Assert.AreEqual(1, _loadResult.Data.Count);

        #region 1st Record Tests
        [Test]
        public void Then_Account_Is_Returned() =>
            Assert.AreEqual(new Guid("9082609f-9cf8-e811-80e0-000d3a214f60"), _firstRecord.Account);

        [Test]
        public void Then_Company_Name_Is_Returned() =>
            Assert.AreEqual("Company Name", _firstRecord.CompanyName);

        // TODO AU ADD BACK IN
        //[Fact(DisplayName = "1st record - Company Aka is correct")]
        //public void CompanyAkaIsCorrectForRecord1() =>
        //    Assert.Equals("Also Known As", _firstRecord.CompanyAka);

        //[Fact(DisplayName = "1st record - Aupa is correct")]
        //public void AupaIsCorrectForRecord1() =>
        //    Assert.Equals("Aware", _firstRecord.Aupa);

        //[Test]
        //public void Then_Company_Type_Is_Returned() =>
        //    Assert.AreEqual("Employer", _firstRecord.CompanyType);

        [Test]
        public void Then_Phone_Is_Returned() =>
            Assert.AreEqual("7777744465", _firstRecord.Phone);

        [Test]
        public void Then_Email_Is_Returned() =>
            Assert.AreEqual("email@address.com", _firstRecord.Email);

        [Test]
        public void Then_Website_Is_Returned() =>
            Assert.AreEqual("www.website.com", _firstRecord.Website);

        [Test]
        public void Then_Address1_Is_Returned() =>
            Assert.AreEqual("Address1", _firstRecord.Address1);

        [Test]
        public void Then_City_Is_Returned() =>
            Assert.AreEqual("City", _firstRecord.City);

        [Test]
        public void Then_PostCode_Is_Returned() =>
            Assert.AreEqual("S1 1AA", _firstRecord.PostCode);

        [Test]
        public void Then_Created_By_Is_Returned() =>
            Assert.AreEqual("Created By", _firstRecord.CreatedBy);

        [Test]
        public void Then_Created_On_Is_Returned() =>
            Assert.AreEqual(new DateTime(2018, 12, 5, 14, 47, 1), _firstRecord.CreatedOn);

        [Test]
        public void Then_Modified_By_Is_Returned() =>
            Assert.AreEqual("Modified By", _firstRecord.ModifiedBy);

        [Test]
        public void Then_Modified_On_Is_Returned() =>
            Assert.AreEqual(new DateTime(2018, 12, 5, 14, 53, 48), _firstRecord.ModifiedOn);

        [Test]
        public void Then_Owner_Is_Returned() =>
            Assert.AreEqual("Owner", _firstRecord.Owner);
        #endregion
    }
}