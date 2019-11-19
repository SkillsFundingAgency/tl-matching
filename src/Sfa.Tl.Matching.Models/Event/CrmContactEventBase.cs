using Newtonsoft.Json;

namespace Sfa.Tl.Matching.Models.Event
{
    public class CrmContactEventBase
    {
        [JsonProperty("contactid")]
        public string ContactId { get; set; }
        
        [JsonProperty("parentcustomerid")]
        public ParentCustomerId ParentCustomerId { get; set; }

        [JsonProperty("customertypecode")]
        public Customertypecode CustomerTypeCode { get; set; }

        [JsonProperty("fullname")]
        public string Fullname { get; set; }

        [JsonProperty("mobilephone")]
        public object MobilePhone { get; set; }
        
        [JsonProperty("telephone1")]
        public string Telephone { get; set; }

        [JsonProperty("emailaddress1")]
        public string EmailAddress { get; set; }

        [JsonProperty("address1_city")]
        public string City { get; set; }

        [JsonProperty("address1_county")]
        public string County { get; set; }

        [JsonProperty("address1_line1")]
        public string AddressLine1 { get; set; }

        [JsonProperty("address1_line2")]
        public string AddressLine2 { get; set; }

        [JsonProperty("address1_line3")]
        public object AddressLine3 { get; set; }

        [JsonProperty("address1_postalcode")]
        public string PostCode { get; set; }
    }

    public class ParentCustomerId
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("logicalname")]
        public string LogicalName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}