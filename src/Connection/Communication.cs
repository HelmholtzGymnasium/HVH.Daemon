/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2017
 * Licensed under the terms of the MIT License
 */

using System;

namespace HVH.Service.Connection
{
    /// <summary>
    /// The communication protocol for Client and Server
    /// </summary>
    public static class Communication
    {
        /// <summary>
        /// Empty Text
        /// </summary>
        public const String Empty = "Empty";

        /// <summary>
        /// Indentification ID for the program
        /// </summary>
        public const String DAEMON_ID = "9033fa9938484bf9ae1b27b4ff9ed8de";

        /// <summary>
        /// Announcement that the service is going to send his public key to the server
        /// </summary>
        public const String DAEMON_SEND_PUBLIC_KEY = "198edb63031e422fbde1760bb13bd87a";

        /// <summary>
        /// After sending this, the service will send informations about the machine he is running on
        /// </summary>
        public const String DAEMON_SEND_SESSION_DATA = "68d986b0ec164908ab060aae242aefac";

        /// <summary>
        /// Service sends a signal that he is still alive
        /// </summary>
        public const String DAEMON_SEND_HEARTBEAT = "65e52334b16b4f70a1066001a708769f";

        /// <summary>
        /// Service disconnects from the server
        /// </summary>
        public const String DAEMON_SEND_DISCONNECT = "7afca47069b949b285f91c6fa0fc3b65";

        /// <summary>
        /// Identification ID for the server program
        /// </summary>
        public const String SERVER_ID = "1079f3e4be3647dcae9302e73b7604d5";

        /// <summary>
        /// Server will send a session key for symmetrical encryption next
        /// </summary>
        public const String SERVER_SEND_SESSION_KEY = "3b635b374e3f4060bdaba35090a117a8";

        /// <summary>
        /// Confirmation that the service has registered with the server
        /// </summary>
        public const String SERVER_SEND_SESSION_CREATED = "7289d8a91dcb456b871c428d164f53ba";

        /// <summary>
        /// Server requests a response from the client, to keep the connection active
        /// </summary>
        public const String SERVER_SEND_HEARTBEAT_CHALLENGE = "e14d04eca02d4c8cb8d3c79c5b720158";

        /// <summary>
        /// Server tells the client to shut down
        /// </summary>
        public const String SERVER_SEND_SHUTDOWN = "c6d1f3ff26bb4038b2c7d18456fbd6f1";

        /// <summary>
        /// Server tells the client to reboot
        /// </summary>
        public const String SERVER_SEND_REBOOT = "52fe0e9ff2774712b6ea3b76e5775aef";

        /// <summary>
        /// Server sends a list of forbidden processes, that will be added to the existing list
        /// </summary>
        public const String SERVER_SEND_FORBIDDEN_PROCESSES = "7bd5e72b747a463986443378dd725fe4";

        /// <summary>
        /// Same as SERVER_SEND_FORBIDDEN_PROCESSES, except that the list will be completely cleared
        /// </summary>
        public const String SERVER_SEND_FORBIDDEN_PROCESSES_CLEAR = "5fc8feb816d440fda22a18b73e36b22a";

        /// <summary>
        /// Server asks the client to wait for the next package
        /// </summary>
        public const String SERVER_SEND_WAIT_SIGNAL = "f9b257264e2e4cd4be4f16fc3d06037b";

        /// <summary>
        /// Server goes offline
        /// </summary>
        public const String SERVER_SEND_DISCONNECT = "b503a21db88c40a0b2195cbdc913651b";

        /// <summary>
        /// Server sends the command to lock the client pc
        /// </summary>
        public const String SERVER_SEND_LOCK = "d9bffdc37f634621932619165a661686";

        /// <summary>
        /// Server sends the command to unlock the client pc
        /// </summary>
        public const String SERVER_SEND_UNLOCK = "b369a2aa5bbc46a59bd1bcde6bb6bb63";

        /**
         * -> Client Connects to Server
         * -> CLIENT_SEND_PUBLIC_KEY -- Public Key
         * -> SERVER_SEND_SESSION_KEY -- Session Key
         * -> CLIENT_SEND_SESSION_DATA -- PC Name and Username + Client ID
         * -> SERVER_SEND_SESSION_CREATED -- Server ID
         * -> ...
         * In a loop:
         *      -> SERVER_SEND_HEARTBEAT_CHALLENGE -- Random value
         *      -> CLIENT_SEND_HEARTBEAT -- Same random value
         */
    }
}