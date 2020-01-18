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
    RemoteClients[] remoteClients;

    private int localClientID = -1;

    public Client()
    {
        this.commandDictionary = new CommandDictionary();

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
            Debug.Log("HOSTNAME: " + hostName);
            localClient.Connect(hostName, connectPort);
        }
        
        Message messageObject = new Message(localClientID, 0);
        string connectRequest = messageObject.constructMessage();

        localClient.Send(Encoding.ASCII.GetBytes(connectRequest), connectRequest.Length);
    }


    // thread for listening for server responses
    private void socketListen()
    {      
        while (true)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] buffer = localClient.Receive(ref endPoint);
            Debug.Log("CLIENT: " + Encoding.ASCII.GetString(buffer));

            parseServerMessage(Encoding.ASCII.GetString(buffer));

        }
    }


    // OH SHIT IM GOING TO FUCKING CHUM IN MY PANNNTS AUGGGH
    private void parseServerMessage(string message)
    {
        Message messageObject = JsonUtility.FromJson<Message>(message);
        
        if(messageObject.message == 2)
        {
            connectionAccepted(message);
        }
    }

   
    // once the server receives the connection request and accepts the client this method updates the local id
    private void connectionAccepted(string message)
    {
        ConnectMessage connectMessage = JsonUtility.FromJson<ConnectMessage>(message);
        localClientID = connectMessage.newLocalID;

    }
}
