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

    //MainThreadQueue mainThreadQueue = new MainThreadQueue();

    Queue<string> mainThreadMessageQueue;
    ClientSceneManager sceneManager;

    private int localClientID = -1;

    public Client(ClientSceneManager sceneManager)
    {
        this.commandDictionary = new CommandDictionary();
        this.remoteClients = new List<RemoteClients>();
        this.mainThreadMessageQueue = new Queue<string>();

        this.sceneManager = sceneManager;
    }
  

    // Attempts to create a connection to the server with given host and port
    public void ConnectToServer(int connectPort, string hostName)
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
        
        Message messageObject = new Message(localClientID, 0);
        string connectRequest = messageObject.constructMessage();

        //localClient.Send(Encoding.ASCII.GetBytes(connectRequest), connectRequest.Length);
        sendToServer(connectRequest);
    }

    public void mainThread()
    {

        if(mainThreadMessageQueue.Count > 0)
        {
            // ******* TODO: Change this to a switch statement later when more main thread commands are needed
            string nextMessage = mainThreadMessageQueue.Dequeue();
            PositionUpdateMessage positionUpdateMessage = JsonUtility.FromJson<PositionUpdateMessage>(nextMessage);
            foreach(RemoteClients remote in remoteClients)
            {
                if(remote.clientID == positionUpdateMessage.clientID)
                {
                    //Debug.LogWarning("Client " + localClientID + ": " + "Changed client " + remote.clientID + " to position " + positionUpdateMessage.position.ToString());
                    remote.clientTransform = positionUpdateMessage.position;
                    remote.clientRotation = positionUpdateMessage.rotation;
                    //sceneManager.updateRemote();
                }
            }
        }
      
    }


    // Updates the location of position on the server and communicates it to all connected clients
    public void sendPositionUpdate(Vector3 position, Quaternion rotation)
    {
        PositionUpdateMessage positionUpdateMessage = new PositionUpdateMessage(localClientID, 4, position, rotation);
        sendToServer(positionUpdateMessage.constructMessage());
        
    }

    private void sendToServer(string message)
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
            case 2:
                connectionAccepted(message);
                break;
            case 3:
                addNewClient(message);
                break;
            case 4:
                setClientTransform(message);
                break;
        }
    }

   
    // once the server receives the connection request and accepts the client this method updates the local id
    private void connectionAccepted(string message)
    {
        ConnectMessage connectMessage = JsonUtility.FromJson<ConnectMessage>(message);
        localClientID = connectMessage.newLocalID;

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
        /*PositionUpdateMessage positionUpdateMessage = JsonUtility.FromJson<PositionUpdateMessage>(message);

        Debug.Log(localClientID.ToString() +  message);

        mainThreadQueue.SetPosition(positionUpdateMessage.transform, positionUpdateMessage.clientID);*/

        mainThreadMessageQueue.Enqueue(message);

    }
}
