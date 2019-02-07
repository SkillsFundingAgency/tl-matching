using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedMember.Global
namespace Sfa.Tl.Matching.Domain.Models
{
    public class RoutePathMapping
    {
        [Key]
        public int Id { get; set; }
        public string LarsId { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public int PathId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual Path Path { get; set; }
    }
}