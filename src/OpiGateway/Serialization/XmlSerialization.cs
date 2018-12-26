using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace OpiGateway.Serialization
{
    /// <summary>
    /// Static utility methods for serializing objects to, and deserializing from, UTF8-encoded XML
    /// </summary>
    public static class XmlSerialization
    {
        /// <summary>
        /// Serialize an object to a UTF-8 XML string
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="obj">The object</param>
        /// <returns>XML string</returns>
        public static string Serialize<T>(T obj)
        {
            if (obj == null)
            {
                throw new NullReferenceException($"Cannot serialize NULL for {typeof(T).Name}");
            }
            
            string serial;

            var xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty); // always remove namespace

            var serializer = new XmlSerializer(typeof(T));
            var stream = new MemoryStream();
            var encoding = new UTF8Encoding(false); // suppress UTF-8 byte order mark
            using (var writer = new XmlTextWriter(stream, encoding))
            {
                serializer.Serialize(writer, obj, xmlNamespaces);
                serial = Encoding.UTF8.GetString(stream.ToArray(), 0, (int)stream.Length);
            }

            return serial;
        }

        /// <summary>
        /// Deserialize an object from an XML string
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="input">The XML string</param>
        /// <returns>The deserialized object</returns>
        public static T Deserialize<T>(string input) where T : new()
        {
            if (input == null)
            {
                throw new NullReferenceException($"Cannot deserialize NULL as {typeof(T).Name}");
            }
            
            T obj;
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(input.Trim()))
            {
                obj = (T)serializer.Deserialize(reader);
            }

            return obj;
        }
    }
}