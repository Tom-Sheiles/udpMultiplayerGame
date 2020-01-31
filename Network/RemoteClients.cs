using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class RemoteClients {

    public int clientID;
    public IPEndPoint clientEndPoint;
    public Vector3 clientTransform;
    public Quaternion clientRotation;
    public string playerName;
    

    public RemoteClients(int id)
    {
        this.clientID = id;
    }

    public void setEndPoint(IPEndPoint endpoint)
    {
        this.clientEndPoint = endpoint;
    }

    public int getID() { return this.clientID; }
    public int getPort() { return this.clientEndPoint.Port; }
}
