using System.Xml.Serialization;

namespace OpiGateway.Dto
{
    [XmlRoot(ElementName = "POSData")]
    public class PosData
    {
        [XmlElement(ElementName = "POSTimeStamp")]
        public string PosTimeStamp { get; set; }

        [XmlElement(ElementName = "ClerkID")]
        public string ClerkId { get; set; }

        [XmlElement(ElementName = "TransactionNumber")]
        public string TransactionNumber { get; set; }

        [XmlAttribute(AttributeName = "LanguageCode")]
        public string LanguageCode { get; set; }
    }
}