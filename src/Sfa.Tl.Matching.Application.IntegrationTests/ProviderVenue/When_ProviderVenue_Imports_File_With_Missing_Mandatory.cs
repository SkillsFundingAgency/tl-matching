﻿//using System.IO;
//using System.Threading.Tasks;
//using NUnit.Framework;
//using Sfa.Tl.Matching.Models.Dto;

//namespace Sfa.Tl.Matching.Application.IntegrationTests.ProviderVenue
//{
//    public class When_ProviderVenue_Imports_File_With_Missing_Mandatory : ProviderVenueTestBase
//    {
//        private const string DataFilePath = @"ProviderVenue\ProviderVenue-MissingMandatory.xlsx";
//        private int _createdRecordCount;

//        [SetUp]
//        public async Task Setup()
//        {
//            await ResetData();

//            var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, DataFilePath);
//            using (var stream = File.Open(filePath, FileMode.Open))
//            {
//                _createdRecordCount = ProviderVenueService.ImportProviderVenue(new ProviderVenueFileImportDto { FileDataStream = stream }).Result;
//            }
//        }

//        [Test]
//        public void Then_No_Record_Is_Saved()
//        {
//            Assert.AreEqual(0, _createdRecordCount);
//        }
//    }
//}