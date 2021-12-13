using System.Text.Json.Serialization;

namespace Sfa.Tl.Matching.Models.Event
{
    public class CrmEmployerEventBase
    {
        public string ContactEmail { get; set; }
        public string ContactTelephone1 { get; set; }
        public string Name { get; set; }
        public PrimaryContactId PrimaryContactId { get; set; }

        [JsonPropertyName("accountid")]
        public string AccountId { get; set; }

        [JsonPropertyName("address1_line1")]
        public string AddressLine { get; set; }

        [JsonPropertyName("address1_postalcode")]
        public string PostCode { get; set; }

        [JsonPropertyName("customertypecode")]
        public Customertypecode CustomerTypeCode { get; set; }

        [JsonPropertyName("emailaddress1")]
        public object EmailAddress { get; set; }

        [JsonPropertyName("owneremail")]
        public string OwnerEmail { get; set; }

        [JsonPropertyName("owneridname")]
        public string OwnerIdName { get; set; }

        [JsonPropertyName("sfa_alias")]
        public string Alias { get; set; }

        [JsonPropertyName("sfa_aupa")]
        // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
        public SfaAupa sfa_aupa { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        [JsonPropertyName("sfa_ceasedtrading")]
        public int CeasedTrading { get; set; }

        [JsonPropertyName("sfa_employermanagement")]
        public object EmployerManagement { get; set; }

        [JsonPropertyName("telephone1")]
        public string Phone { get; set; }
    }

    public class PrimaryContactId
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("logicalname")]
        public string LogicalName { get; set; }

        [JsonPropertyName("name")]
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