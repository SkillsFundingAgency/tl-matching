using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class UserCache : BaseEntity
    {
        public string UserCacheKey { get; set; }
        public string UrlHistory { get; set; }

        [NotMapped]
        public List<CurrentUrl> Value
        {
            get => UrlHistory == null ? null : JsonConvert.DeserializeObject<List<CurrentUrl>>(UrlHistory);
            set => UrlHistory = JsonConvert.SerializeObject(value);
        }

        [NotMapped]
        public CacheTypes Key
        {
            get
            {
                Enum.TryParse(UserCacheKey, out CacheTypes result);
                return result;
            }

            set => UserCacheKey = value.ToString();
        }
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