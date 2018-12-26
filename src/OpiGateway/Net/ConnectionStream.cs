using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace OpiGateway.Net
{
    /// <inheritdoc />
    /// <summary>
    /// A communication channel over a TCP/IP network stream
    /// </summary>
    public class ConnectionStream : IDisposable
    {
        private const int MessageLengthPrefixBytes = 4;
        private readonly NetworkStream stream;
        private bool dispose = false; // detect redundant calls

        /// <summary>
        /// Instantiate a new communication channel for a given TCP/IP-based network stream
        /// </summary>
        public ConnectionStream(NetworkStream stream)
        {
            this.stream = stream;
        }
        
        /// <summary>
        /// Read a message from the network stream, as bytes
        /// </summary>
        /// <param name="bufferSize">The size of the reading buffer, in bytes</param>
        /// <returns>The message read, in bytes, in the form of an asynchronous task</returns>
        /// <exception cref="Exception">If the message appears to be malformed</exception>
        public async Task<byte[]> ReadAsync(int bufferSize)
        {
            byte[] message;
            
            using (var memory = new MemoryStream())
            {
                var buffer = new byte[bufferSize];
                var read = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (read <= MessageLengthPrefixBytes)
                {
                    throw new Exception("Invalid message: Not enough information to extract message length prefix");
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
                    throw new Exception("Invalid message: Bad value for message length prefix");
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
        
        /// <summary>
        /// Write a message on the client communication channel
        /// </summary>
        /// <param name="message">The message to send to the client, as bytes</param>
        public async Task WriteAsync(byte[] message)
        {
            var prefix = BitConverter.GetBytes(message.Length);
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