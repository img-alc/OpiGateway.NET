using System.Xml.Serialization;

namespace OpiGateway.Dto
{
    [XmlRoot(ElementName = "CardServiceResponse")]
    public class CardServiceResponse
    {
        [XmlElement(ElementName = "Terminal")]
        public Terminal Terminal { get; set; }

        [XmlElement(ElementName = "Tender")]
        public Tender Tender { get; set; }

        [XmlAttribute(AttributeName = "RequestType")]
        public string RequestType { get; set; }

        [XmlAttribute(AttributeName = "ApplicationSender")]
        public string ApplicationSender { get; set; }

        [XmlAttribute(AttributeName = "WorkstationID")]
        public string WorkstationId { get; set; }

        [XmlAttribute(AttributeName = "RequestID")]
        public string RequestId { get; set; }

        [XmlAttribute(AttributeName = "OverallResult")]
        public string OverallResult { get; set; }
    }
}