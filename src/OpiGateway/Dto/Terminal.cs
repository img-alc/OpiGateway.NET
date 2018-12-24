using System.Xml.Serialization;

namespace OpiGateway.Dto
{
    [XmlRoot(ElementName = "Terminal")]
    public class Terminal
    {
        [XmlAttribute(AttributeName = "TerminalID")]
        public string TerminalId { get; set; }

        [XmlAttribute(AttributeName = "TerminalBatch")]
        public string TerminalBatch { get; set; }

        [XmlAttribute(AttributeName = "STAN")]
        public string Stan { get; set; }
    }
}