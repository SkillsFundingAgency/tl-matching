using System;

// ReSharper disable UnusedMember.Global
namespace Sfa.Tl.Matching.Domain.Models
{
    public class RoutePathMapping
    {
        public int Id { get; set; }
        public string LarsId { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public int PathId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual Path Path { get; set; }
    }
}