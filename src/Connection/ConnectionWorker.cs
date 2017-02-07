/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2017
 * Licensed under the terms of the MIT License
 */

using System;
using System.Net;
using Helios.Exceptions;
using Helios.Net;
using Helios.Net.Bootstrap;
using Helios.Topology;

namespace HVH.Service.Connection
{
    public class ConnectionWorker
    {
        /// <summary>
        /// The connection to the server
        /// </summary>
        public IConnection Client { get; private set; }

        /// <summary>
        /// The node for the server connection
        /// </summary>
        private INode RemoteHost { get; set; }

        /// <summary>
        /// The hostname of the server we are connected to
        /// </summary>
        public String Server { get; }

        /// <summary>
        /// The port on the server where the endpoint listens
        /// </summary>
        public Int32 Port { get; }

        /// <summary>
        /// Callback that is invoked when the connection is closed
        /// </summary>
        public Action<HeliosConnectionException, IConnection> Terminated { get; set; }

        /// <summary>
        /// Callback that is invoked when data is received
        /// </summary>
        public Action<NetworkData, IConnection> Received { get; set; }

        /// <summary>
        /// Callback that is invoked when the connection to the server was successful
        /// </summary>
        public Action<INode, IConnection> Established { get; set; }

        public ConnectionWorker(String server, Int32 port)
        {
            Server = server;
            Port = port;

            // Create the client
            RemoteHost = NodeBuilder.BuildNode().Host(Server).WithPort(port).WithTransportType(TransportType.Tcp);
            Client = new ClientBootstrap()
                .SetTransport(TransportType.Tcp)
                .RemoteAddress(RemoteHost)
                .OnConnect(ConnectionEstablishedCallback)
                .OnReceive(ReceivedDataCallback)
                .OnDisconnect(ConnectionTerminatedCallback)
                .Build()
                .NewConnection(NodeBuilder.BuildNode().Host(IPAddress.Any).WithPort(port), RemoteHost);
        }

        private void ConnectionTerminatedCallback(HeliosConnectionException reason, IConnection closedchannel)
        {
            Terminated?.Invoke(reason, closedchannel);
        }

        private void ReceivedDataCallback(NetworkData incomingdata, IConnection responsechannel)
        {
            Received?.Invoke(incomingdata, responsechannel);
        }

        private void ConnectionEstablishedCallback(INode remoteaddress, IConnection responsechannel)
        {
            Established?.Invoke(remoteaddress, responsechannel);
        }
    }
}