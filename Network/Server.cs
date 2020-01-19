using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Server
{

    private const int port = 3000;
    private int numberOfconnectedClients;
    private UdpClient udp;

    CommandDictionary commandDictionary;
    List<RemoteClients> remoteClients;


    public Server()
    {
        this.commandDictionary = new CommandDictionary();
        this.remoteClients = new List<RemoteClients>();
        this.numberOfconnectedClients = 0;
    }


    // Creates the thread responsible for all host server logic
    public void StartServer()
    {
        Thread thread = new Thread(new ThreadStart(serverThread));
        thread.Start();
    }


    // Thread initializes the udp connection and recieves incoming client messages
    private void serverThread()
    {
        udp = new UdpClient(port);
        Debug.Log("Server Started on " + port);

        while (true)
        {
            IPEndPoint clientEndpoint = new IPEndPoint(IPAddress.Any, 0);

            byte[] buffer = udp.Receive(ref clientEndpoint);
            debugendpoint(clientEndpoint);
            string clientMessage = Encoding.ASCII.GetString(buffer);

            parseClientMessage(clientEndpoint, clientMessage);
        }
    }


    // Parses an incoming JSON string into a Message object
    private void parseClientMessage(IPEndPoint client, string message)
    {
        Debug.Log("SERVER: " + message);

        Message messageObject = JsonUtility.FromJson<Message>(message);

        if(messageObject.message == 0 && messageObject.clientID == -1)
        {
            assignClientID(client);

        }else if(messageObject.message == 4)
        {
            updatePosition(message);
        }
    }


    // Called when a client connect request is sent from a new client. assigns a new id and responds with the current game state
    private void assignClientID(IPEndPoint client)
    {
        RemoteClients newRemoteClient = new RemoteClients(numberOfconnectedClients);
        ConnectMessage connectMessage = new ConnectMessage(-2, 2, numberOfconnectedClients);
        newRemoteClient.setEndPoint(client);


        sendToClient(client, connectMessage.constructMessage());

        // Send all existing players to the new player
        foreach(RemoteClients remote in remoteClients)
        {
            NewPlayerMessage existingPlayerMessage = new NewPlayerMessage(-2, 3, remote.clientID);
            sendToClient(client, existingPlayerMessage.constructMessage());
        }

        remoteClients.Add(newRemoteClient);


        // Send the new client joined to all existing clients
        for (int i = 0; i < numberOfconnectedClients; i++)
        {
            NewPlayerMessage newPlayerMessage = new NewPlayerMessage(-2, 3, numberOfconnectedClients);
            sendToClient(remoteClients[i].clientEndPoint, newPlayerMessage.constructMessage());
        }

        numberOfconnectedClients++;

    }

    
    private void updatePosition(string message)
    {
        PositionUpdateMessage positionUpdate = JsonUtility.FromJson<PositionUpdateMessage>(message);
        
       foreach(RemoteClients remote in remoteClients)
        {
            if(remote.clientID != positionUpdate.clientID)
            {
                sendToClient(remote.clientEndPoint, positionUpdate.constructMessage());
            }
        }
    }


    // Sends a UDP Message to the supplied client, must be a JSON string of the Message class or subclasses
    private void sendToClient(IPEndPoint client, string JsonString)
    {
        udp.Send(Encoding.ASCII.GetBytes(JsonString), JsonString.Length, client);
    }


    private static void debugendpoint(IPEndPoint endpoint)
    {
        //Debug.Log("Address: " + endpoint.Address);
        //Debug.Log("Address Family: " + endpoint.AddressFamily);
        //Debug.Log("Port: " + endpoint.Port);
        //Debug.Log("CONNECT INFO: " + endpoint.ToString());

    }

    
}
