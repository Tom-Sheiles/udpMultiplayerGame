using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class RemoteClients {

    int clientID;
    private IPEndPoint clientEndPoint;
    

    public RemoteClients(int id)
    {
        this.clientID = id;
    }

    public void setEndPoint(IPEndPoint endpoint)
    {
        this.clientEndPoint = endpoint;
    }
}
