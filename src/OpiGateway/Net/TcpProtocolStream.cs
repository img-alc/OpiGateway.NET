using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace OpiGateway.Net
{
    /// <inheritdoc />
    /// <summary>
    /// A TCP/IP network connection stream capable of reading and writing OPI protocol messages
    /// </summary>
    public class TcpProtocolStream : ITcpProtocolStream
    {
        private const int MessageLengthPrefixBytes = 4;
        private readonly ITcpConnectionStream stream;
        private bool dispose; // detect redundant calls

        /// <summary>
        /// A new OPI protocol stream over a TCP/IP-based network stream
        /// </summary>
        public TcpProtocolStream(ITcpConnectionStream stream)
        {
            this.stream = stream;
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Read an OPI protocol message from the TCP/IP network stream, as bytes
        /// </summary>
        /// <param name="bufferSize">The size of the reading buffer, in bytes</param>
        /// <returns>The OPI protocol message read, in bytes, in the form of an asynchronous task</returns>
        /// <exception cref="T:System.IO.IOException">If the OPI message appears to be malformed, or invalid</exception>
        public async Task<byte[]> ReadAsync(int bufferSize)
        {
            byte[] message;
            
            using (var memory = new MemoryStream())
            {
                var buffer = new byte[bufferSize];
                var read = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (read <= MessageLengthPrefixBytes)
                {
                    throw new IOException("Invalid message: Not enough information to extract message length prefix");
                }

                var prefix = new byte[MessageLengthPrefixBytes];
                Array.Copy(buffer, prefix, MessageLengthPrefixBytes);
                if (BitConverter.IsLittleEndian) // most significant byte on the right
                {
                    Array.Reverse(prefix);
                }

                var msgLength = BitConverter.ToInt32(prefix, 0);
                if (msgLength <= 0)
                {
                    throw new IOException($"Invalid message: Bad value {msgLength} for message length prefix");
                }

                memory.Write(buffer, MessageLengthPrefixBytes, read - MessageLengthPrefixBytes);
                var remaining = msgLength - MessageLengthPrefixBytes - read;
                while (remaining > 0)
                {
                    read = await stream.ReadAsync(buffer, 0, buffer.Length);
                    memory.Write(buffer, 0, read);
                    remaining -= read;
                }
                message = memory.ToArray();
            }

            return message;
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Write an OPI protocol message on the TCP/IP network stream, as bytes
        /// </summary>
        /// <param name="message">The OPI message to send to the client, as bytes</param>
        public async Task WriteAsync(byte[] message)
        {
            if (message == null)
            {
                throw new NullReferenceException("Cannot write NULL as message");
            }
            
            var prefix = BitConverter.GetBytes(message.Length);
            if (BitConverter.IsLittleEndian) // most significant byte on the right
            {
                Array.Reverse(prefix);
            }
            var prefixed = new byte[message.Length + MessageLengthPrefixBytes];
            
            Array.Copy(prefix, prefixed, MessageLengthPrefixBytes);
            // BlockCopy slightly faster than Array.Copy in this case
            Buffer.BlockCopy(message, 0, prefixed, MessageLengthPrefixBytes, message.Length);

            await stream.WriteAsync(prefixed, 0, prefixed.Length);
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