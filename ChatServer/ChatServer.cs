﻿// <copyright file="ChatServer.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

using CS3500.Networking;
using System.Text;

namespace CS3500.Chatting;

/// <summary>
///   A simple ChatServer that handles clients separately and replies with a static message.
/// </summary>
public partial class ChatServer
{

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
        var name = "User"; // Default name
        // handle all messages until disconnect.
        try
        {
            connection.Send("Please type your name: ");
            name = connection.ReadLine();
            Server.SendMessageToAllClients(name + " has connected to the server!");
            while ( true )
            {
                var message = connection.ReadLine( );
                Server.SendMessageToAllClients(name + ": " + message);
            }
        }
        catch ( Exception )
        {
            connection.Disconnect();
            Server.SendMessageToAllClients(name + " has disconnected.");
        }
    }

    
}