using System;
using System.Threading.Tasks;
using OpiGateway.Net;

namespace OpiGateway.Tests.Net
{
    /// <inheritdoc />
    /// <summary>
    /// A fake, mock implementation of a <see cref="ITcpConnectionStream"/>, which does basically nothing
    /// </summary>
    public class NoOpTcpConnectionStream : ITcpConnectionStream
    {
        /// <summary>
        /// A specific set of bytes to be returned every time a read is requested
        /// </summary>
        private readonly byte[] input;

        /// <summary>
        /// The set of bytes this stream has attempted to write after a write has been requested
        /// </summary>
        private byte[] output;

        /// <summary>
        /// Instantiated with a specific set of bytes to be returned every time a read is requested
        /// </summary>
        /// <param name="input">The byte sequence to return every time a read is requested</param>
        public NoOpTcpConnectionStream(byte[] input)
        {
            this.input = input;
        }

        /// <inheritdoc />
        /// <summary>
        /// Act like reading something, from somewhere, but actually fill the buffer with a specific set of bytes.
        /// </summary>
        public async Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            return await Task.Run(() =>
            {
                var length = input.Length < count ? input.Length : count;
                Array.Copy(input, buffer, length);
                
                return length;
            });
        }

        /// <inheritdoc />
        /// <summary>
        /// Writes nothing to nowhere, does essentially nothing
        /// </summary>
        public async Task WriteAsync(byte[] buffer, int offset, int count)
        {
            await Task.Run(() =>
            {
                output = new byte[buffer.Length];
                Array.Copy(buffer, output, output.Length);
            });
        }

        /// <summary>
        /// Retrieve the last sequence of bytes this stream has attempted to write
        /// </summary>
        /// <returns>The set of bytes this stream has attempted to write</returns>
        public byte[] GetLastOutput()
        {
            return output;
        }

        /// <inheritdoc />
        /// <summary>
        /// No managed or unmanaged resources, doesn't do anything really
        /// </summary>
        public void Dispose()
        {
            // nothing to dispose
        }
    }
}