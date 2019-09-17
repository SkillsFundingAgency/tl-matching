namespace Sfa.Tl.Matching.Models.Event
{
    public class CrmContactEventBase
    {
        public string ContactEmail { get; set; }
        public string ContactTelephone1 { get; set; }
        public string Name { get; set; }
        public PrimaryContactId PrimaryContactId { get; set; }
        public string accountid { get; set; }
        public string address1_line1 { get; set; }
        public string address1_postalcode { get; set; }
        public Customertypecode customertypecode { get; set; }
        public object emailaddress1 { get; set; }
        public string owneremail { get; set; }
        public string owneridname { get; set; }
        public string sfa_alias { get; set; }
        public SfaAupa sfa_aupa { get; set; }
        public int sfa_ceasedtrading { get; set; }
        public object sfa_employermanagement { get; set; }
        public string telephone1 { get; set; }
    }
}