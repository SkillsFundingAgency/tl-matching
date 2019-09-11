using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class UserCacheDto
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public List<CurrentUrl> Value { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}