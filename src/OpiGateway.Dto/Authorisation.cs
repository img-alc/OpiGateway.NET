using System.Xml.Serialization;

namespace OpiGateway.Dto
{
    [XmlRoot(ElementName = "Authorisation")]
    public class Authorisation
    {
        [XmlAttribute(AttributeName = "CardPAN")]
        public string CardPan { get; set; }

        [XmlAttribute(AttributeName = "ExpiryDate")]
        public string ExpiryDate { get; set; }

        [XmlAttribute(AttributeName = "TimeStamp")]
        public string TimeStamp { get; set; }

        [XmlAttribute(AttributeName = "ApprovalCode")]
        public string ApprovalCode { get; set; }

        [XmlAttribute(AttributeName = "CardCircuit")]
        public string CardCircuit { get; set; }

        [XmlAttribute(AttributeName = "ResponseCode")]
        public string ResponseCode { get; set; }
    }
}