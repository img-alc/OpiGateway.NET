using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpiGateway.Dto;
using OpiGateway.Serialization;

namespace OpiGateway.Tests.Serialization
{
    [TestClass]
    public class XmlSerializationUTests
    {
        [TestMethod]
        public void Serialize_ShouldThrowNullReferenceException_WhenObjectIsNull()
        {
            Assert.ThrowsException<NullReferenceException>(() => XmlSerialization.Serialize<CardServiceRequest>(null));
        }

        [TestMethod]
        public void Deserialize_ShouldThrowNullReferenceException_WhenStringIsNull()
        {
            Assert.ThrowsException<NullReferenceException>(() => XmlSerialization.Deserialize<CardServiceResponse>(null));
        }
        
        [TestMethod]
        public void Serialize_ShouldReturnXmlWithoutNamespaceAndUtf8ByteOrderMark_WhenObjectNotNull()
        {
            const string expected = "<?xml version=\"1.0\" encoding=\"utf-8\"?><CardServiceResponse />";
            var actual = XmlSerialization.Serialize(new CardServiceResponse());
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Deserialize_ShouldReturnTypeInstance_WhenXmlNotNull()
        {
            const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><CardServiceRequest />";
            var obj = XmlSerialization.Deserialize<CardServiceRequest>(xml);
            
            Assert.IsNotNull(obj);
        }
    }
}