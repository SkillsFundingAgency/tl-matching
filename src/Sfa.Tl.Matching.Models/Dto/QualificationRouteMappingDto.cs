﻿
// ReSharper disable UnusedMember.Global
namespace Sfa.Tl.Matching.Models.Dto
{
    public class QualificationRouteMappingDto
    {
        public QualificationDto Qualification { get; set; }
        public int RouteId { get; set; }
        public int QualificationId { get; set; }
        public string Source { get; set; }
        public string CreatedBy { get; set; }
    }
}