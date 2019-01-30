using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;
using Sfa.Tl.Matching.Infrastructure.Enums;
using Sfa.Tl.Matching.Web.ViewModels;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class DataImportViewModelMapper : IDataImportViewModelMapper
    {
        public DataImportViewModel Populate()
        {
            var viewModel = new DataImportViewModel
            {
                DataImportTypeViewModels = CreateDataImportTypeViewModels()
            };

            return viewModel;
        }

        #region Private Methods
        private static List<DataImportTypeViewModel> CreateDataImportTypeViewModels()
        {
            var dataImportTypeNames = Enum.GetNames(typeof(DataImportType));

            var dataImportTypeViewModels = dataImportTypeNames.Select(uploadType =>
                new DataImportTypeViewModel
                {
                    Id = GetId(uploadType),
                    Name = GetDescription(uploadType)
                }).ToList();

            return dataImportTypeViewModels;
        }

        private static int GetId(string uploadType) =>
            (int)Enum.Parse(typeof(DataImportType), uploadType);

        private static string GetDescription(string uploadType) =>
            ((DataImportType)Enum.Parse(typeof(DataImportType), uploadType)).Humanize();
        #endregion
    }
}