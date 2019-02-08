using System.Xml.Serialization;

namespace OpiGateway.Dto
{
    [XmlRoot(ElementName = "TotalAmount")]
    public class TotalAmount
    {
        [XmlAttribute(AttributeName = "PaymentAmount")]
        public string PaymentAmount { get; set; }

        [XmlAttribute(AttributeName = "Currency")]
        public string Currency { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
}