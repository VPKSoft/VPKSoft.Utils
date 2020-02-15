#region License
/*
VPKSoft.Utils

Some utilities by VPKSoft.
Copyright © 2020 VPKSoft, Petteri Kautonen

Contact: vpksoft@vpksoft.net

This file is part of VPKSoft.Utils.

VPKSoft.Utils is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

VPKSoft.Utils is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with VPKSoft.Utils.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace VPKSoft
{
    namespace Utils
    {
        /// <summary>
        /// An interface used by RemotingMessageClient and RemotingMessageServer classes.
        /// </summary>
        public interface IMessageSender
        {
            /// <summary>
            /// A method skeleton ipmplemented by RemotingMessageClient and RemotingMessageServer classes.
            /// </summary>
            /// <param name="message">A string message to send or receive.</param>
            /// <param name="tcpPort">A tcp port to be used for the communication.</param>
            /// <param name="uriExtension">An uri extension e.g. "messageTest" to use for the communication.</param>
            /// <returns>True if the message between RPC (Remote Procedure Call) client and server was exchanged successfully, otherwise false.</returns>
            bool SendMessage(string message, int tcpPort, string uriExtension);
        }

        /// <summary>
        /// A helper class for RemotingMessageClient and RemotingMessageServer classes
        /// <para/>to check if TCP channel is registed with System.Runtime.Remoting.ChannelServices
        /// <para/>class static member called RegisteredChannels.
        /// </summary>
        public class RemotingMessageHelper
        {
            /// <summary>
            /// Returns true if a channed named "tcp" is registered within
            /// <para/>System.Runtime.Remoting.ChannelServices.RegisteredChannels
            /// <para/>array.
            /// </summary>
            /// <returns>True if the channed "tcp" was registered, otherwise false.</returns>
            public static bool TCPChannelRegistered()
            {
                bool tcpRegistered = false;
                foreach (IChannel chan in ChannelServices.RegisteredChannels)
                {
                    if (chan.ChannelName == "tcp")
                    {
                        tcpRegistered = true;
                    }
                }
                return tcpRegistered;
            }
        }

        /// <summary>
        /// Provides an inter-process communication server using TCP protocol and .NET remoting.
        /// <para/>This part will receive the messages and fires events as they are received.
        /// </summary>
        public class RemotingMessageServer : MarshalByRefObject, IMessageSender
        {
            /// <summary>
            /// An internal class to hold a properties of a TCP
            /// <para/>channel used for .NET remoting.
            /// </summary>
            private class ServerEntry
            {
                /// <summary>
                /// TCP port number to be used.
                /// </summary>
                private int tctPort = 0;

                /// <summary>
                /// An uri extension e.g. "messageTest" to use for the communication.
                /// </summary>
                private string uriExtension = string.Empty;

                /// <summary>
                /// An event that is fired when RemotingMessageServer.SendMessage gets called.
                /// </summary>
                private OnMessage onMessageEvent = null;

                /// <summary>
                /// A limit for number of messages before the queue count
                /// <para/>for messages is considered overflown before they
                /// <para/>could be delegated via the GotMessage event.
                /// </summary>
                private int floodLimit = 100;

                /// <summary>
                /// A limit of characters in a message before the message 
                /// <para/>is considered overflown.
                /// </summary>
                private int maxChars = 10000;

                /// <summary>
                /// Gets or sets the value indicating if the ServerEntry class instance 
                /// <para/>properties were read by a SendMessage method implementation of 
                /// <para/>the RemotingMessageServer class instance.
                /// </summary>
                public bool GotParams { get; set; }

                /// <summary>
                /// A TcpChannel given in the class constructor.
                /// </summary>
                private TcpChannel channel = null;

                /// <summary>
                /// Unregisters the the TcpChannel channel if registered in the ChannelServices.
                /// </summary>
                public void UnregisterTCPChannel()
                {
                    try
                    {
                        if (RemotingMessageHelper.TCPChannelRegistered())
                        {
                            ChannelServices.UnregisterChannel(channel);
                        }
                    }
                    catch
                    {

                    }
                }

                /// <summary>
                /// A TcpChannel given in the class constructor.
                /// </summary>
                public TcpChannel Channel
                {
                    get
                    {
                        return channel;
                    }
                }

                /// <summary>
                /// A limit for number of messages before the queue count
                /// <para/>for messages is considered overflown before they
                /// <para/>could be delegated via the GotMessage event.
                /// </summary>
                public int FloodLimit
                {
                    get
                    {
                        return floodLimit;
                    }
                }

                /// <summary>
                /// A limit of characters in a message before the message 
                /// <para/>is considered overflown.
                /// </summary>
                public int MaxChars
                {
                    get
                    {
                        return maxChars;
                    }
                }

                /// <summary>
                /// A TCP port to be used with the communication.
                /// </summary>
                public int TCPPort
                {
                    get
                    {
                        return tctPort;
                    }
                }

                /// <summary>
                /// An uri extension e.g. "messageTest" to use for the communication.
                /// </summary>
                public string UriExtension
                {
                    get
                    {
                        return uriExtension;
                    }
                }

                /// <summary>
                /// An event that is fired when a RPC (Remote Procedure Call) is received.
                /// </summary>
                public OnMessage OnMessageEvent
                {
                    get
                    {
                        return onMessageEvent;
                    }
                }

                /// <summary>
                /// A constructor for the ServerEntry class.
                /// </summary>
                /// <param name="TCPPort">TCP port number to be used for the communication.</param>
                /// <param name="UriExtension">An uri extension e.g. "messageTest" to use for the communication.</param>
                /// <param name="OnMessageEvent">An event that is fired when a RPC (Remote Procedure Call) is received.</param>
                /// <param name="Channel">A TcpChannel which was ininitialized with a TCP port number.</param>
                /// <param name="FloodLimit">A limit for number of messages before the queue count
                /// <para/>for messages is considered overflown before they
                /// <para/>could be delegated via the GotMessage event.</param>
                /// <param name="MaxChars">A limit of characters in a message before the message 
                /// <para/>is considered overflown.</param>
                public ServerEntry(int TCPPort, string UriExtension, OnMessage OnMessageEvent, TcpChannel Channel, int FloodLimit = 1000, int MaxChars = 10000)
                {
                    channel = Channel;
                    tctPort = TCPPort;
                    uriExtension = UriExtension;
                    onMessageEvent = OnMessageEvent;
                    floodLimit = FloodLimit;
                    maxChars = MaxChars;
                }

                /// <summary>
                /// A Hepler function that checks if an instance of this class matches with given TCP port and uri excension.
                /// </summary>
                /// <param name="TCPPort">A TCP port number to check.</param>
                /// <param name="UriExtension">An uri extension to check.</param>
                /// <returns>True if the class instance matched the given parameters, otherwise false.</returns>
                public bool Match(int TCPPort, string UriExtension)
                {
                    return (tctPort == TCPPort) && (uriExtension == UriExtension);
                }

                /// <summary>
                /// Gets a ServerEntry class instance that matches with given parameters.
                /// </summary>
                /// <param name="servers">A list of ServerEntry class instances to check.</param>
                /// <param name="TCPPort">A TCP port number to check.</param>
                /// <param name="UriExtension">An uri extension to check.</param>
                /// <returns>A ServerEntry class instance if one matching the given parametes
                /// <para/>was found in the given ServerEntry class instance list, otherwise null.</returns>
                public static ServerEntry GetMatch(List<ServerEntry> servers, int TCPPort, string UriExtension)
                {
                    foreach (ServerEntry se in servers)
                    {
                        if (se.Match(TCPPort, UriExtension))
                        {
                            return se;
                        }
                    }
                    return null;
                }

                /// <summary>
                /// Gets a value indicating if a ServerEntry class instance exists that matches with given parameters.
                /// </summary>
                /// <param name="servers">A list of ServerEntry class instances to check.</param>
                /// <param name="TCPPort">A TCP port number to check.</param>
                /// <param name="UriExtension">An uri extension to check.</param>
                /// <returns>True if one matching the given parametes
                /// <para/>was found in the given ServerEntry class instance list, otherwise false.</returns>
                public static bool ListMatch(List<ServerEntry> servers, int TCPPort, string UriExtension)
                {
                    foreach (ServerEntry se in servers)
                    {
                        if (se.Match(TCPPort, UriExtension))
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            /// <summary>
            /// An internal static list of RPC (Remote Procedure Call) servers 
            /// <para/>registered with an this class using Register method.
            /// </summary>
            private static List<ServerEntry> servers = new List<ServerEntry>();

            /// <summary>
            /// A delegate for the GotMessage event.
            /// </summary>
            /// <param name="message">A string message to pass with the GotMessage event.</param>
            public delegate void OnMessage(string message);

            /// <summary>
            /// An event which is fired when a RPC (Remote Procedure Call) is received.
            /// </summary>
            public event OnMessage GotMessage = null;

            /// <summary>
            /// A list of message that couldn't be passed via the GotMessage
            /// <para/>event because it wasn't assigned.
            /// </summary>
            private List<string> messageQueue = new List<string>();

            /// <summary>
            /// A TcpChannel used for the communication.
            /// </summary>
            private TcpChannel channel = null;

            /// <summary>
            /// A TCP port number to be used for the communication.
            /// </summary>
            private int tctPort = 0;

            /// <summary>
            /// An uri extension e.g. "messageTest" to use for the communication.
            /// </summary>
            private string uriExtension = string.Empty;

            /// <summary>
            /// A limit for number of messages before the queue count
            /// <para/>for messages is considered overflown before they
            /// <para/>could be delegated via the GotMessage event.
            /// </summary>
            private int floodLimit = 100;

            /// <summary>
            /// A limit of characters in a message before the message 
            /// <para/>is considered overflown.
            /// </summary>
            private int maxChars = 10000;

            /// <summary>
            /// Registers a TcpChannel with a given port number, adds a ServerEntry class instance
            /// <para/>to the internal static list and registers the RPC (Remote Procedure Call) server
            /// <para/>with the RemotingConfiguration class.
            /// </summary>
            /// <param name="TCPPort">A TCP port number to be used for the communication.</param>
            /// <param name="UriExtension">An uri extension e.g. "messageTest" to use for the communication.</param>
            /// <param name="msg">A deletegate for the event when a message is received.</param>
            /// <param name="FloodLimit">A limit for number of messages before the queue count
            /// <para/>for messages is considered overflown before they
            /// <para/>could be delegated via the GotMessage event.</param>
            /// <param name="MaxChars">A limit of characters in a message before the message 
            /// <para/>is considered overflown.</param>
            /// <returns>True if a new ServerEntry class instance was successfully registered, Otherwise (and on Exception) false.</returns>
            public static bool Register(int TCPPort, string UriExtension, OnMessage msg, int FloodLimit = 1000, int MaxChars = 10000)
            {
                try
                {
                    TcpChannel channel = new TcpChannel(TCPPort);
                    if (!ServerEntry.ListMatch(servers, TCPPort, UriExtension))
                    {
                        servers.Add(new ServerEntry(TCPPort, UriExtension, msg, channel, FloodLimit, MaxChars));
                    }
                    else
                    {
                        return false;
                    }

                    if (!RemotingMessageHelper.TCPChannelRegistered())
                    {
                        ChannelServices.RegisterChannel(channel, false);
                    }

                    RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemotingMessageServer), UriExtension, WellKnownObjectMode.Singleton);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            /// <summary>
            /// Un-registers a TcpChannel with a given port number, removes a ServerEntry class instance
            /// <para/>from the internal static list.
            /// </summary>
            /// <param name="TCPPort">A TCP port number used with the communication.</param>
            /// <param name="UriExtension">An uri extension e.g. "messageTest" used for the communication.</param>
            /// <returns></returns>
            public static bool UnRegister(int TCPPort, string UriExtension)
            {
                for (int i = servers.Count - 1; i >= 0; i--)
                {
                    if (servers[i].Match(TCPPort, UriExtension))
                    {
                        servers[i].GotParams = false;
                        servers.RemoveAt(i);
                        return true;
                    }
                }
                return false;
            }

            /// <summary>
            /// A queue of messages in list string form.
            /// </summary>
            public List<string> MessageQueue
            {
                get
                {
                    return messageQueue;
                }
            }

            /// <summary>
            /// Gets a value indicating if there are un-delegated messages in the queue.
            /// </summary>
            /// <returns>True if there are un-delegated messages in the queue, otherwise false.</returns>
            public bool MessagesInQueue()
            {
                return messageQueue.Count > 0;
            }


            /// <summary>
            /// Gets a first pending message from the message queue.
            /// </summary>
            /// <returns>A string message if one was found from the queue, otherwise string.Empty.</returns>
            public string PopQueue()
            {
                string retval = string.Empty;
                if (MessagesInQueue())
                {
                    retval = messageQueue[0];
                    messageQueue.RemoveAt(0);
                }
                return retval;
            }

            /// <summary>
            /// Delegates a received message by firing the GotMessage event.
            /// <para/>If the floodLimit or maxChars were exceeded, and exception is thrown.
            /// </summary>
            /// <param name="message">A message to delegate.</param>
            /// <param name="tcpPort">A TCP port from which the message was received.</param>
            /// <param name="uriExtension">An uri extension from which the message was received.</param>
            /// <returns>True if the message was successfully delegated, otherwise false.
            /// <para/><remarks>An exception is thrown if the message limits were exceeded.</remarks>
            /// </returns>
            public bool SendMessage(string message, int tcpPort, string uriExtension)
            {
                ServerEntry se = ServerEntry.GetMatch(servers, tcpPort, uriExtension);
                if (se != null)
                {
                    if (!se.GotParams)
                    {
                        GotMessage = se.OnMessageEvent;
                        floodLimit = se.FloodLimit;
                        maxChars = se.MaxChars;
                        channel = se.Channel;

                        tctPort = se.TCPPort;
                        uriExtension = se.UriExtension;
                        se.GotParams = true;
                    }
                }
                if (messageQueue.Count > floodLimit)
                {
                    throw new Exception("Message server flood limit message was exceeded. Message queue max length is " + floodLimit + " (tcp: " + tctPort + ", uri: '" + uriExtension + "').");
                }

                if (message.Length > maxChars)
                {
                    throw new Exception("Maximum amout of characters was in a message was exceeded: " + maxChars + " (tcp: " + tctPort + ", uri: '" + uriExtension + "').");
                }

                try
                {
                    if (GotMessage != null)
                    {
                        GotMessage(message);
                    }
                    else
                    {
                        messageQueue.Add(message);
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

        /// <summary>
        /// A RPC (Remote Procedure Call) client that sends messages to a
        /// <para/>corresponding client.
        /// </summary>
        public class RemotingMessageClient : IMessageSender
        {
            /// <summary>
            /// A TCP port number to be used for the communication.
            /// </summary>
            private int tcpPort = 0;

            /// <summary>
            /// An uri extension e.g. "messageTest" to use for the communication.
            /// </summary>
            private string uriExtension = string.Empty;

            /// <summary>
            /// A TcpChannel used for the communication.
            /// </summary>
            private TcpChannel channel = null;

            /// <summary>
            /// An IMessageSender interface method created by an Activator class
            /// <para/>to pass messages to the server.
            /// </summary>
            private IMessageSender clientMessaging = null;

            /// <summary>
            /// A method to un-register a TCP channel used by this class instance.
            /// </summary>
            public void UnregisterTCPChannel()
            {
                try
                {
                    if (RemotingMessageHelper.TCPChannelRegistered())
                    {
                        ChannelServices.UnregisterChannel(channel);

                    }
                }
                catch
                {

                }
            }

            /// <summary>
            /// Gets the TCP port number given in the class constructor.
            /// </summary>
            public int TCPPort
            {
                get
                {
                    return tcpPort;
                }
            }

            /// <summary>
            /// Gets the uri extension given in the class constructor.
            /// </summary>
            public string UriExtension
            {
                get
                {
                    return uriExtension;
                }
            }

            /// <summary>
            /// A constructor for a RPC (Remote Procedure Call) client.
            /// </summary>
            /// <param name="TCPPort">A TCP port number to be used for the communication.</param>
            /// <param name="UriExtension">An uri extension to be used for the communication.</param>
            public RemotingMessageClient(int TCPPort, string UriExtension)
            {
                tcpPort = TCPPort;
                uriExtension = UriExtension;
                try
                {
                    channel = new TcpChannel();
                    if (!RemotingMessageHelper.TCPChannelRegistered())
                    {
                        ChannelServices.RegisterChannel(channel, false);
                    }


                    clientMessaging = (IMessageSender)Activator.GetObject(typeof(IMessageSender), "tcp://localhost:" + tcpPort + "/" + uriExtension);
                }
                catch
                {
                    channel = null;
                    clientMessaging = null;
                }
            }

            /// <summary>
            /// Sends a message to a RPC (Remote Procedure Call) server with
            /// <para/>class instances TCP port number and uri extension.
            /// </summary>
            /// <param name="message">A message to send.</param>
            /// <returns>True if the message was sent successfully, otherwise false.</returns>
            public bool SendMessage(string message)
            {
                return SendMessage(message, tcpPort, uriExtension);
            }


            /// <summary>
            /// Send a message to a RPC (Remote Procedure Call) server with
            /// <para/>a given TCP port number and uri extension.
            /// </summary>
            /// <param name="message">A message to send.</param>
            /// <param name="tcpPort">A TCP port to use for the communication.</param>
            /// <param name="uriExtension">An uri extension to use for the communication.</param>
            /// <returns>True if the message was sent successfully, otherwise false.</returns>
            public bool SendMessage(string message, int tcpPort, string uriExtension)
            {
                if (clientMessaging == null)
                {
                    return false;
                }

                try
                {
                    clientMessaging.SendMessage(message, tcpPort, uriExtension);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}