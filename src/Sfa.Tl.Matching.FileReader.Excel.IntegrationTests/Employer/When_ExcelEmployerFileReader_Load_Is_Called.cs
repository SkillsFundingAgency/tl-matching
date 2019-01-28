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
            Assert.AreEqual(new Guid("9082609f-9cf8-e811-80e0-000d3a214f60"), _firstRecord.CrmId);

        [Test]
        public void Then_Company_Name_Is_Returned() =>
            Assert.AreEqual("Company Name", _firstRecord.CompanyName);

        [Test]
        public void Then_Phone_Is_Returned() =>
            Assert.AreEqual("7777744465", _firstRecord.Phone);

        [Test]
        public void Then_Email_Is_Returned() =>
            Assert.AreEqual("email@address.com", _firstRecord.Email);

        [Test]
        public void Then_PostCode_Is_Returned() =>
            Assert.AreEqual("S1 1AA", _firstRecord.PostCode);

        [Test]
        public void Then_Owner_Is_Returned() =>
            Assert.AreEqual("Owner", _firstRecord.Owner);
        #endregion
    }
}