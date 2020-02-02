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
    private bool isInProgress;

    CommandDictionary commandDictionary;
    List<RemoteClients> remoteClients;

    Queue<string> mainThreadMessageQueue;


    public Server()
    {
        this.commandDictionary = new CommandDictionary();
        this.remoteClients = new List<RemoteClients>();
        this.numberOfconnectedClients = 0;
        this.mainThreadMessageQueue = new Queue<string>();
        this.isInProgress = true;
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


    public void mainThread()
    {
        if(mainThreadMessageQueue.Count > 0)
        {
            // ******* TODO: Change this to a switch statement later when more main thread commands are needed
            string nextMessage = mainThreadMessageQueue.Dequeue();
            updatePosition(nextMessage);
            
        }
    }


    // Parses an incoming JSON string into a Message object
    private void parseClientMessage(IPEndPoint client, string message)
    {
        Message messageObject = JsonUtility.FromJson<Message>(message);

        switch (messageObject.message)
        {
            case (int)Message.messageTypes.ConnectRequest:
                assignClientID(client, message);
                Debug.Log("SERVER: " + message);
                break;

            case (int)Message.messageTypes.PositionUpdate:
                mainThreadMessageQueue.Enqueue(message);
                break;

            case (int)Message.messageTypes.RaycastMessage:
                raycastHit(message);
                break;
            case (int)Message.messageTypes.InstantiateObject:
                sendToAllClients(message);
                break;
        }
    }


    // Called when a client connect request is sent from a new client. assigns a new id and responds with the current game state
    private void assignClientID(IPEndPoint client, string message)
    {
        RemoteClients newRemoteClient = new RemoteClients(numberOfconnectedClients);
        ConnectMessage connectMessage = new ConnectMessage(-2, numberOfconnectedClients, isInProgress);
        Message recievedMessage = JsonUtility.FromJson<Message>(message);
        newRemoteClient.setEndPoint(client);


        sendToClient(client, connectMessage.constructMessage());

        // Send all existing players to the new player
        foreach(RemoteClients remote in remoteClients)
        {
            NewPlayerMessage existingPlayerMessage = new NewPlayerMessage(-2, remote.clientID, remote.playerName);
            sendToClient(client, existingPlayerMessage.constructMessage());
        }

        remoteClients.Add(newRemoteClient);


        // Send the new client joined to all existing clients
        for (int i = 0; i < numberOfconnectedClients; i++)
        {
            NewPlayerMessage newPlayerMessage = new NewPlayerMessage(-2, numberOfconnectedClients, recievedMessage.clientName);
            sendToClient(remoteClients[i].clientEndPoint, newPlayerMessage.constructMessage());
        }

        numberOfconnectedClients++;

    }

    
    // sends the updated position of a player to other clients
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


    // Sends a message to a client hit by a raycast from another client
    private void raycastHit(string message)
    {
        RaycastHitMessage raycastHitMessage = JsonUtility.FromJson<RaycastHitMessage>(message);
        foreach(RemoteClients remote in remoteClients)
        {
            if(remote.clientID == raycastHitMessage.targetID)
            {
                sendToClient(remote.clientEndPoint, message);
            }
        }
        
    }


    // Sends a UDP Message to the supplied client, must be a JSON string of the Message class or subclasses
    private void sendToClient(IPEndPoint client, string JsonString)
    {
        udp.Send(Encoding.ASCII.GetBytes(JsonString), JsonString.Length, client);
    }


    // Sends a UDP Message to all clients currently on the server.
    private void sendToAllClients(string message)
    {
        foreach(RemoteClients remote in remoteClients)
        {
            udp.Send(Encoding.ASCII.GetBytes(message), message.Length, remote.clientEndPoint);
        }
    }


    private static void debugendpoint(IPEndPoint endpoint)
    {
        //Debug.Log("Address: " + endpoint.Address);
        //Debug.Log("Address Family: " + endpoint.AddressFamily);
        //Debug.Log("Port: " + endpoint.Port);
        //Debug.Log("CONNECT INFO: " + endpoint.ToString());

    }

    
}
