using System.Xml.Serialization;

namespace OpiGateway.Dto
{
    [XmlRoot(ElementName = "Tender")]
    public class Tender
    {
        [XmlElement(ElementName = "TotalAmount")]
        public TotalAmount TotalAmount { get; set; }

        [XmlElement(ElementName = "Authorisation")]
        public Authorisation Authorisation { get; set; }
    }
}