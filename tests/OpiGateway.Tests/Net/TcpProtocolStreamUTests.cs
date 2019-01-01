using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpiGateway.Net;

namespace OpiGateway.Tests.Net
{
    [TestClass]
    public class TcpProtocolStreamUTests
    {   
        [TestMethod]
        public void ReadAsync_ShouldThrowIOException_WhenNotEnoughBytesMessageLengthPrefix()
        {
            var stream = new NoOpTcpConnectionStream(new byte[2]);
            var channel = new TcpProtocolStream(stream);

            var ex = Assert.ThrowsExceptionAsync<IOException>(() => channel.ReadAsync(4096)).Result;
            
            Assert.AreEqual("Invalid message: Not enough information to extract message length prefix", ex.Message);
        }
        
        [TestMethod]
        public void ReadAsync_ShouldThrowIOException_WhenMessageHasZeroLengthPrefix()
        {
            var stream = new NoOpTcpConnectionStream(new byte[4096]);
            var channel = new TcpProtocolStream(stream);

            var ex = Assert.ThrowsExceptionAsync<IOException>(() => channel.ReadAsync(4096)).Result;
            
            Assert.AreEqual("Invalid message: Bad value 0 for message length prefix", ex.Message);
        }

        [TestMethod]
        public void ReadAsync_ShouldNotThrow_WhenMessageLengthPrefixGreaterThanActualMessageLength()
        {
            var stream = new NoOpTcpConnectionStream(new byte[] { 0, 0, 0, 4, 0, 0 });
            var channel = new TcpProtocolStream(stream);

            var read = channel.ReadAsync(6).Result;
            
            Assert.AreEqual(2, read.Length);
            Assert.AreEqual(0, read[0]);
            Assert.AreEqual(0, read[1]);
        }
        
        [TestMethod]
        public void ReadAsync_ShouldNotTruncateMessage_WhenMessageLengthPrefixLessThanActualMessageLength()
        {
            var stream = new NoOpTcpConnectionStream(new byte[] { 0, 0, 0, 2, 0, 0, 0, 0 });
            var channel = new TcpProtocolStream(stream);

            var read = channel.ReadAsync(8).Result;
            
            Assert.AreEqual(4, read.Length);
            Assert.AreEqual(0, read[0]);
            Assert.AreEqual(0, read[1]);
            Assert.AreEqual(0, read[2]);
            Assert.AreEqual(0, read[3]);
        }
        
        [TestMethod]
        public void ReadAsync_ShouldTruncateMessage_WhenBufferSizeLessThanMessageLength()
        {
            var stream = new NoOpTcpConnectionStream(new byte[] { 0, 0, 0, 4, 0, 0, 0, 0 });
            var channel = new TcpProtocolStream(stream);

            var read = channel.ReadAsync(6).Result;
            
            Assert.AreEqual(2, read.Length);
            Assert.AreEqual(0, read[0]);
            Assert.AreEqual(0, read[1]);
        }
        
        [TestMethod]
        public void ReadAsync_ShouldReadEntireMessageWithLengthPrefix_WhenMessageExistsWithLengthPrefix()
        {
            var stream = new NoOpTcpConnectionStream(new byte[] { 0, 0, 0, 4, 0, 0, 0, 1 });
            var channel = new TcpProtocolStream(stream);            
            
            var read = channel.ReadAsync(8).Result;
            
            Assert.AreEqual(4, read.Length);
            Assert.AreEqual(0, read[0]);
            Assert.AreEqual(0, read[1]);
            Assert.AreEqual(0, read[2]);
            Assert.AreEqual(1, read[3]);
        }

        [TestMethod]
        public void WriteAsync_ShouldWriteMessageWithLengthPrefix_WhenMessageIsNotNull()
        {
            var stream = new NoOpTcpConnectionStream(new byte[0] /* irrelevant */);
            var channel = new TcpProtocolStream(stream);

            channel.WriteAsync(new byte[] {0, 1, 0, 1}).Wait();

            var written = stream.GetLastOutput();
            
            Assert.AreEqual(8, written.Length);
            Assert.AreEqual(0, written[0]);
            Assert.AreEqual(0, written[1]);
            Assert.AreEqual(0, written[2]);
            Assert.AreEqual(4, written[3]);
            Assert.AreEqual(0, written[4]);
            Assert.AreEqual(1, written[5]);
            Assert.AreEqual(0, written[6]);
            Assert.AreEqual(1, written[7]);
        }
        
        [TestMethod]
        public void WriteAsync_ShouldThrowNullReferenceException_WhenMessageIsNull()
        {
            var stream = new NoOpTcpConnectionStream(new byte[0] /* irrelevant */);
            var channel = new TcpProtocolStream(stream);

            var ex = Assert.ThrowsExceptionAsync<NullReferenceException>(() => channel.WriteAsync(null)).Result;

            Assert.AreEqual("Cannot write NULL as message", ex.Message);
        }
    }
}