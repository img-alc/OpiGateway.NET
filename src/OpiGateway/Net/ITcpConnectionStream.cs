using System;
using System.Threading.Tasks;

namespace OpiGateway.Net
{
    /// <inheritdoc />
    /// <summary>
    /// A wrapper over a TCP/IP network connection stream, capable of reading and writing
    /// </summary>
    public interface ITcpConnectionStream : IDisposable
    {
        /// <summary>
        /// Read a series of bytes into a byte buffer
        /// </summary>
        /// <param name="buffer">The buffer, an array of bytes</param>
        /// <param name="offset">Number of bytes to skip in reading</param>
        /// <param name="count">Total number of bytes to read</param>
        /// <returns>The number of bytes read, in the form of an asynchronous <see cref="Task"/></returns>
        Task<int> ReadAsync(byte[] buffer, int offset, int count);

        /// <summary>
        /// Write a series of bytes onto the stream
        /// </summary>
        /// <param name="buffer">The buffer, array of bytes to be written</param>
        /// <param name="offset">Number of bytes to skip in writing</param>
        /// <param name="count">Total number of bytes to write</param>
        /// <returns>Nothing, as an asynchronous <see cref="Task" /></returns>
        Task WriteAsync(byte[] buffer, int offset, int count);
    }
}