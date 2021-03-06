﻿using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class DataUploadDto
    {
        public DataImportType ImportType { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string FileName { get; set; }
        public string UserName { get; set; }
    }
}