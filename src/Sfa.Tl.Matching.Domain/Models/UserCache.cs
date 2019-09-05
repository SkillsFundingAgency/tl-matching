using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class UserCache : BaseEntity
    {
        public string Key { get; set; }
        public string UrlHistory { get; set; }

        [NotMapped]
        public CurrentUrl Value
        {
            get => UrlHistory == null ? null : JsonConvert.DeserializeObject<CurrentUrl>(UrlHistory);
            set => UrlHistory = JsonConvert.SerializeObject(value);
        }
    }

    public class CurrentUrl
    {
        public int Id { get; set; }
        public string Url { get; set; }
    }
}