using Newtonsoft.Json;

namespace Sfa.Tl.Matching.Models.Event
{
    public class CrmEmployerEventBase
    {
        public string ContactEmail { get; set; }
        public string ContactTelephone1 { get; set; }
        public string Name { get; set; }
        public PrimaryContactId PrimaryContactId { get; set; }

        [JsonProperty("accountid")]
        public string AccountId { get; set; }

        [JsonProperty("address1_line1")]
        public string AddressLine { get; set; }

        [JsonProperty("address1_postalcode")]
        public string PostCode { get; set; }

        [JsonProperty("customertypecode")]
        public Customertypecode CustomerTypeCode { get; set; }

        [JsonProperty("emailaddress1")]
        public object EmailAddress { get; set; }

        [JsonProperty("owneremail")]
        public string OwnerEmail { get; set; }

        [JsonProperty("owneridname")]
        public string OwnerIdName { get; set; }

        [JsonProperty("sfa_alias")]
        public string Alias { get; set; }

        [JsonProperty("sfa_aupa")]
        // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
        public SfaAupa sfa_aupa { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        [JsonProperty("sfa_ceasedtrading")]
        public int CeasedTrading { get; set; }

        [JsonProperty("sfa_employermanagement")]
        public object EmployerManagement { get; set; }

        [JsonProperty("telephone1")]
        public string Phone { get; set; }
    }

    public class PrimaryContactId
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("logicalname")]
        public string LogicalName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Customertypecode
    {
        public int Value { get; set; }
    }

    public class SfaAupa
    {
        public int Value { get; set; }
    }
}