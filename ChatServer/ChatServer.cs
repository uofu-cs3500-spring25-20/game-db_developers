// <copyright file="ChatServer.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

using CS3500.Networking;
using System.Net.Sockets;
using System.Text;

namespace CS3500.Chatting;

/// <summary>
///   A simple ChatServer that handles clients separately and replies with a static message.
/// </summary>
public partial class ChatServer
{

    /// <summary>
    /// List of the clients.
    /// </summary>
    private static List<NetworkConnection> clients = new();


    /// <summary>
    ///   The main program.
    /// </summary>
    /// <param name="args"> ignored. </param>
    /// <returns> A Task. Not really used. </returns>
    private static void Main( string[] args )
    {
        Server.StartServer( HandleConnect, 11_000 );
        Console.Read(); // don't stop the program.
    }


    /// <summary>
    ///   <pre>
    ///     When a new connection is established, enter a loop that receives from and
    ///     replies to a client.
    ///   </pre>
    /// </summary>
    ///
    private static void HandleConnect( NetworkConnection connection )
    {
        AddClient(connection);
        var name = "User"; // Default name
        // handle all messages until disconnect.
        try
        {
            connection.Send("Please type your name: ");
            name = connection.ReadLine();
            SendMessageToAllClients(name + " has connected to the server!");
            while ( true )
            {
                var message = connection.ReadLine( );
                if (!message.Equals(string.Empty))
                    SendMessageToAllClients(name + ": " + message);
            }
        }
        catch ( Exception )
        {
            RemoveClient(connection);
            connection.Disconnect();
        }
    }

    /// <summary>
    /// Adds a connection to the list of clients.
    /// </summary>
    /// <param name="connection">Connection to add.</param>
    public static void AddClient(NetworkConnection connection)
    {
        lock (clients)
        {
            clients.Add(connection);
        }
    }

    /// <summary>
    /// Removes the client from the list of clients to broadcast to.
    /// </summary>
    /// <param name="connection">Connection to remove.</param>
    public static void RemoveClient(NetworkConnection connection)
    {
        lock (clients)
        {
            clients.Remove(connection);
        }
    }

    /// <summary>
    /// This method sends a message to all clients on the server.
    /// </summary>
    /// <param name="message">Message to be sent.</param>
    public static void SendMessageToAllClients(string message)
    {
        lock (clients)
        {
            foreach (NetworkConnection c in clients)
            {
                c.Send(message);
            }
        }
    }

}