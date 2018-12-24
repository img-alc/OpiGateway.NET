using System;
using System.Xml.Serialization;

namespace OpiGateway.Dto
{
    [XmlRoot(ElementName = "CardServiceRequest")]
    public class CardServiceRequest
    {
        [XmlElement(ElementName = "POSData")]
        public PosData PosData { get; set; }

        [XmlElement(ElementName = "PrivateData")]
        public string PrivateData { get; set; }

        [XmlElement(ElementName = "ReversalReceiptNo")]
        public string ReversalReceiptNo { get; set; }

        [XmlElement(ElementName = "TotalAmount")]
        public TotalAmount TotalAmount { get; set; }

        [XmlElement(ElementName = "CardValue")]
        public string CardValue { get; set; }

        [XmlAttribute(AttributeName = "RequestType")]
        public string RequestType { get; set; }

        [XmlAttribute(AttributeName = "ApplicationSender")]
        public string ApplicationSender { get; set; }

        [XmlAttribute(AttributeName = "WorkstationID")]
        public string WorkstationId { get; set; }

        [XmlAttribute(AttributeName = "RequestID")]
        public string RequestId { get; set; }
    }
}