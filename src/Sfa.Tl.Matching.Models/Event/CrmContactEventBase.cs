namespace Sfa.Tl.Matching.Models.Event
{
    public class CrmContactEventBase
    {
        public string address1_city { get; set; }
        public string address1_county { get; set; }
        public string address1_line1 { get; set; }
        public string address1_line2 { get; set; }
        public object address1_line3 { get; set; }
        public string address1_postalcode { get; set; }
        public string contactid { get; set; }
        public Customertypecode customertypecode { get; set; }
        public string emailaddress1 { get; set; }
        public string fullname { get; set; }
        public object mobilephone { get; set; }
        public Parentcustomerid parentcustomerid { get; set; }
        public string telephone1 { get; set; }
    }

    public class Parentcustomerid
    {
        public string id { get; set; }
        public string logicalname { get; set; }
        public string name { get; set; }
    }
}