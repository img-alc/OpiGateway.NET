using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace OpiGateway.Net
{
    /// <inheritdoc />
    /// <summary>
    /// A TCP/IP-based, <see cref="Socket"/>-powered connection network stream
    /// </summary>
    public class TcpConnectionStream : ITcpConnectionStream
    {
        private readonly TcpClient client;
        private NetworkStream stream;
        private bool dispose; // detect redundant calls

        /// <summary>
        /// Instantiate a new connection stream from a <see cref="TcpClient"/>
        /// </summary>
        /// <param name="client">An already connected <see cref="TcpClient"/></param>
        public TcpConnectionStream(TcpClient client)
        {
            this.client = client;
        }

        /// <inheritdoc />
        /// <summary>
        /// Read a series of bytes into a byte buffer
        /// </summary>
        /// <param name="buffer">The buffer, an array of bytes</param>
        /// <param name="offset">Number of bytes to skip in reading</param>
        /// <param name="count">Total number of bytes to read</param>
        /// <returns>The number of bytes read, in the form of an asynchronous <see cref="Task"/></returns>
        public async Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            return await (stream ?? (stream = client.GetStream())).ReadAsync(buffer, offset, count);
        }

        /// <inheritdoc />
        /// <summary>
        /// Write a series of bytes onto the stream
        /// </summary>
        /// <param name="buffer">The buffer, array of bytes to be written</param>
        /// <param name="offset">Number of bytes to skip in writing</param>
        /// <param name="count">Total number of bytes to write</param>
        /// <returns>Nothing, as an asynchronous <see cref="Task" /></returns>
        public async Task WriteAsync(byte[] buffer, int offset, int count)
        {
            await (stream ?? (stream = client.GetStream())).WriteAsync(buffer, offset, count);
        }

        /// <inheritdoc />
        /// <summary>
        /// Releases all resources used by the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources, and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources, false to release only unmanaged resources</param>
        private void Dispose(bool disposing)
        {
            if (dispose) return;
            if (disposing) stream?.Dispose();

            dispose = true;
        }
    }
}