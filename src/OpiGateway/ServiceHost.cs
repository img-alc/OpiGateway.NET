using System;
using System.Threading.Tasks;
using OpiGateway.Net;

namespace OpiGateway
{
    /// <summary>
    /// The service entry point
    /// </summary>
    public abstract class ServiceHost
    {
        /// <summary>
        /// The connection listener, establishing and managing connections for clients
        /// </summary>
        private readonly ConnectionListener listener;

        /// <summary>
        /// Initializes the service, implementing classes have to invoke one way or another
        /// </summary>
        protected ServiceHost()
        {
            listener = new ConnectionListener(11143); //TODO configurable
        }
        
        /// <summary>
        /// Start the service, begin listening for clients
        /// </summary>
        public void Start()
        {
            Task.Run(() => listener.StartListener());
        }

        /// <summary>
        /// Stop the service, stop listening, close any sockets, and shut down
        /// </summary>
        public void Stop()
        {
            listener.StopListener().Wait();
        }
    }
}