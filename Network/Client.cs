using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


public class Client
{

    UdpClient localClient = new UdpClient(0);
    CommandDictionary commandDictionary;
    public List<RemoteClients> remoteClients;
    NetworkInstantiate networkInst;

    //MainThreadQueue mainThreadQueue = new MainThreadQueue();

    Queue<string> mainThreadMessageQueue;
    ClientSceneManager sceneManager;

    public int localClientID = -1;

    public Client(ClientSceneManager sceneManager)
    {
        this.commandDictionary = new CommandDictionary();
        this.remoteClients = new List<RemoteClients>();
        this.mainThreadMessageQueue = new Queue<string>();

        this.sceneManager = sceneManager;
        networkInst = sceneManager.GetComponent<NetworkInstantiate>();
    }
  

    // Attempts to create a connection to the server with given host and port
    public void ConnectToServer(int connectPort, string hostName, string playerName)
    {

        Thread serverResponseThread = new Thread(new ThreadStart(socketListen));
        serverResponseThread.Start();

        if (hostName == "") {
            hostName = System.Net.Dns.GetHostName();
            localClient.Connect(hostName, connectPort);
        }
        else
        {
            localClient.Connect(hostName, connectPort);
        }

        Message messageObject;

        if (playerName == "")
        {
            messageObject = new Message(localClientID);
           
        }
        else
        {
            messageObject = new Message(localClientID, playerName);

        }

        string connectRequest = messageObject.constructMessage();
        sendToServer(connectRequest);
    }

    public void mainThread()
    {

        if(mainThreadMessageQueue.Count > 0)
        {

            string nextMessage = mainThreadMessageQueue.Dequeue();
            Message messageObject = JsonUtility.FromJson<Message>(nextMessage);

            switch (messageObject.message)
            {
                case (int)Message.messageTypes.ConnectionSuccessful:
                    ConnectMessage connectMessage = JsonUtility.FromJson<ConnectMessage>(nextMessage);
                    if (connectMessage.isInProgress)
                    {
                        sceneManager.spawnLocalPlayer();
                    }
                    else
                    {
                        sceneManager.LobbyMenu();
                    }
                break;

                case (int)Message.messageTypes.PositionUpdate:

                    PositionUpdateMessage positionUpdateMessage = JsonUtility.FromJson<PositionUpdateMessage>(nextMessage);
                    foreach (RemoteClients remote in remoteClients)
                    {
                        if (remote.clientID == positionUpdateMessage.clientID)
                        {
                            remote.clientTransform = positionUpdateMessage.position;
                            remote.clientRotation = positionUpdateMessage.rotation;
                        }
                    }
                break;

                case (int)Message.messageTypes.RaycastMessage:
                    sceneManager.takeDamage(nextMessage);
                break;

                case (int)Message.messageTypes.InstantiateObject:
                    networkInst.recieveInstance(nextMessage);
                break;

            }
        }
      
    }


    // Updates the location of position on the server and communicates it to all connected clients
    public void sendPositionUpdate(Vector3 position, Quaternion rotation)
    {
        PositionUpdateMessage positionUpdateMessage = new PositionUpdateMessage(localClientID, position, rotation);
        sendToServer(positionUpdateMessage.constructMessage());   
    }

    public void sendRaycastHit(RemoteController objectHit, int value)
    {
        RaycastHitMessage raycastHitMessage = new RaycastHitMessage(localClientID, objectHit.id, value);
        sendToServer(raycastHitMessage.constructMessage());
        
    }

    public void sendToServer(string message)
    {
        localClient.Send(Encoding.ASCII.GetBytes(message), message.Length);
    }


    // thread for listening for server responses
    private void socketListen()
    {      
        while (true)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] buffer = localClient.Receive(ref endPoint);
            //Debug.Log("CLIENT ID: " + localClientID + ": " + Encoding.ASCII.GetString(buffer));

            parseServerMessage(Encoding.ASCII.GetString(buffer));

        }
    }


    // OH SHIT IM GOING TO FUCKING CHUM IN MY PANNNTS AUGGGH
    private void parseServerMessage(string message)
    {
        Message messageObject = JsonUtility.FromJson<Message>(message);

        switch (messageObject.message)
        {
            case (int)Message.messageTypes.ConnectionSuccessful:
                connectionAccepted(message);
                break;
            case (int)Message.messageTypes.NewPlayerData:
                addNewClient(message);
                break;
            case (int)Message.messageTypes.PositionUpdate:
                setClientTransform(message);
                break;
            case (int)Message.messageTypes.RaycastMessage:
                raycastHit(message);
                break;
            case (int)Message.messageTypes.InstantiateObject:
                instanceMainThread(message);
                break;
        }
    }

   
    // once the server receives the connection request and accepts the client this method updates the local id
    private void connectionAccepted(string message)
    {
        ConnectMessage connectMessage = JsonUtility.FromJson<ConnectMessage>(message);
        localClientID = connectMessage.newLocalID;

        mainThreadMessageQueue.Enqueue(message);

    }


    // Adds all existing and new remote clients to the local client list of connected clients
    private void addNewClient(string message)
    {
        NewPlayerMessage newPlayerMessage = JsonUtility.FromJson<NewPlayerMessage>(message);
        RemoteClients newClientObject = new RemoteClients(newPlayerMessage.newPlayerID);

        remoteClients.Add(newClientObject);

        sceneManager.instantiateRemote(message);

    }


    private void setClientTransform(string message)
    {
        mainThreadMessageQueue.Enqueue(message);
    }


    private void raycastHit(string message)
    {
        mainThreadMessageQueue.Enqueue(message);
    }

    private void instanceMainThread(string message)
    {
        mainThreadMessageQueue.Enqueue(message);
    }
}
