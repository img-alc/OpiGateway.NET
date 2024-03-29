using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace OpiGateway.Net
{
    /// <summary>
    /// Establishes and manages TCP/IP connections, routes messages through communication channels
    /// </summary>
    public class TcpConnectionListener
    {
        private const int TcpReadBufferSize = 4096;
        private const int TeardownDelayMs = 4000; //TODO configurable
        
        private readonly TcpListener listener;
        private bool listening;
        
        private readonly object sync = new object();
        private readonly IList<Task> connections = new List<Task>(); //TODO connection registry

        /// <summary>
        /// Instantiate a new TCP/IP-based connection listener on a specific port
        /// </summary>
        public TcpConnectionListener(int port)
        {
            listener = TcpListener.Create(port);
        }
        
        /// <summary>
        /// Begin listening for incoming TCP/IP connections
        /// </summary>
        public async Task StartListener()
        {
            listener.Start();
            listening = true;
            while (listening)
            {
                var tcpClient = await listener.AcceptTcpClientAsync();
                var task = RegisterConnectionAsync(tcpClient);
                if (task.IsFaulted) // if already faulted, re-throw any error on the calling context
                {
                    task.Wait();
                }
            }
        }
        
        /// <summary>
        /// Stop listening for connections, close sockets, clean up
        /// </summary>
        public async Task StopListener()
        {
            if (listener.Pending())
            {
                await Task.Delay(TeardownDelayMs);
            }

            try
            {
                listener.Stop();
            }
            catch (Exception)
            {
                //TODO log?
            }
            finally
            {
                listening = false;
            }
        }
        
        /// <summary>
        /// Register a new connection and establish communication with a client
        /// </summary>
        private async Task RegisterConnectionAsync(TcpClient client)
        {
            var connectionTask = HandleConnectionAsync(client);
            lock (sync) connections.Add(connectionTask);
            try
            {
                await connectionTask; // we may be on another thread after "await"
            }
            catch (Exception)
            {
                //TODO log?
            }
            finally
            {
                lock (sync) connections.Remove(connectionTask);
            }
        }
        
        /// <summary>
        /// Handle communication with a client over a TCP/IP connection
        /// </summary>
        private async Task HandleConnectionAsync(TcpClient client)
        {
            await Task.Yield(); // continue asynchronously on another thread
            using (var stream = new TcpProtocolStream(new TcpConnectionStream(client)))
            {
                var request = await stream.ReadAsync(TcpReadBufferSize);
                var response = new byte[] { }; //TODO actual processing

                await stream.WriteAsync(response);
            }
        }
    }
}