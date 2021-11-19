using System.Text.Json.Serialization;

namespace Sfa.Tl.Matching.Models.Event
{
    public class CrmContactEventBase
    {
        [JsonPropertyName("contactid")]
        public string ContactId { get; set; }
        
        [JsonPropertyName("parentcustomerid")]
        public ParentCustomerId ParentCustomerId { get; set; }

        [JsonPropertyName("customertypecode")]
        public Customertypecode CustomerTypeCode { get; set; }

        [JsonPropertyName("fullname")]
        public string Fullname { get; set; }

        [JsonPropertyName("mobilephone")]
        public object MobilePhone { get; set; }
        
        [JsonPropertyName("telephone1")]
        public string Telephone { get; set; }

        [JsonPropertyName("emailaddress1")]
        public string EmailAddress { get; set; }

        [JsonPropertyName("address1_city")]
        public string City { get; set; }

        [JsonPropertyName("address1_county")]
        public string County { get; set; }

        [JsonPropertyName("address1_line1")]
        public string AddressLine1 { get; set; }

        [JsonPropertyName("address1_line2")]
        public string AddressLine2 { get; set; }

        [JsonPropertyName("address1_line3")]
        public object AddressLine3 { get; set; }

        [JsonPropertyName("address1_postalcode")]
        public string PostCode { get; set; }
    }

    public class ParentCustomerId
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("logicalname")]
        public string LogicalName { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}