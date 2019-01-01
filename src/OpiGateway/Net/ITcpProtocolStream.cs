using System;
using System.Threading.Tasks;

namespace OpiGateway.Net
{
    /// <inheritdoc />
    /// <summary>
    /// A TCP/IP network connection stream capable of reading and writing OPI protocol messages
    /// </summary>
    public interface ITcpProtocolStream : IDisposable
    {
        /// <summary>
        /// Read an OPI protocol message from the TCP/IP network stream, as bytes
        /// </summary>
        /// <param name="bufferSize">The size of the reading buffer, in bytes</param>
        /// <returns>The message read, in bytes, in the form of an awaitable task</returns>
        Task<byte[]> ReadAsync(int bufferSize);

        /// <summary>
        /// Write an OPI protocol message on the TCP/IP network stream, as bytes
        /// </summary>
        /// <param name="message">The OPI protocol message to send to the client, as bytes</param>
        Task WriteAsync(byte[] message);
    }
}