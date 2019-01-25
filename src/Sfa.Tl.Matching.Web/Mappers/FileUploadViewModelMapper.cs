using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;
using Sfa.Tl.Matching.Domain.Enums;
using Sfa.Tl.Matching.Web.ViewModels;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class FileUploadViewModelMapper : IFileUploadViewModelMapper
    {
        public FileUploadViewModel Populate()
        {
            var viewModel = new FileUploadViewModel
            {
                FileTypeViewModels = CreateFileTypeViewModels()
            };

            return viewModel;
        }

        #region Private Methods
        private static List<FileTypeViewModel> CreateFileTypeViewModels()
        {
            var fileUploadTypeNames = Enum.GetNames(typeof(FileUploadType));

            var fileTypeViewModels = fileUploadTypeNames.Select(uploadType =>
                new FileTypeViewModel
                {
                    Id = GetId(uploadType),
                    Name = GetDescription(uploadType),
                }).ToList();

            return fileTypeViewModels;
        }

        private static int GetId(string uploadType) =>
            (int)Enum.Parse(typeof(FileUploadType), uploadType);

        private static string GetDescription(string uploadType) =>
            ((FileUploadType)Enum.Parse(typeof(FileUploadType), uploadType)).Humanize();
        #endregion
    }
}