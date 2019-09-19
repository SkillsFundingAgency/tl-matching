using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class UserCache : BaseEntity
    {
        public string UrlHistory { get; set; }

        [NotMapped]
        public List<CurrentUrl> Value
        {
            get => UrlHistory == null ? null : JsonConvert.DeserializeObject<List<CurrentUrl>>(UrlHistory);
            set => UrlHistory = JsonConvert.SerializeObject(value);
        }

        public string Key { get; set; }
    }

    public class CurrentUrl
    {
        public int Id { get; set; }
        public string Url { get; set; }
    }
    public enum CacheTypes
    {
        BackLink = 1
    }
}