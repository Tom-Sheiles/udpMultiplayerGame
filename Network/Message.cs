using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Message
{
    public int clientID;
    public int message;
    public string clientName;

    public enum messageTypes
    {
        ConnectRequest,
        DisconnectRequest,
        ConnectionSuccessful,
        NewPlayerData,
        PositionUpdate,
        RaycastMessage
    }

    public Message(int clientID, string clientName)
    {
        this.clientID = clientID;
        this.message = (int)messageTypes.ConnectRequest;
        this.clientName = clientName;
    }

    public Message(int clientID)
    {
        this.clientID = clientID;
        this.message = (int)messageTypes.ConnectRequest;
        this.clientName = "NewPlayer";
    }

    public Message() { }

    public string constructMessage()
    {
        return JsonUtility.ToJson(this);
    }
}



public class ConnectMessage: Message
{
    public int newLocalID;

    public ConnectMessage(int clientID, int newID)
    {
        this.clientID = clientID;
        this.message = (int)messageTypes.ConnectionSuccessful;
        this.newLocalID = newID;
    }
}



public class NewPlayerMessage: Message
{
    public int newPlayerID;

    public NewPlayerMessage(int clientID, int id, string clientName)
    {
        this.clientID = clientID;
        this.message = (int)messageTypes.NewPlayerData;
        this.newPlayerID = id;
        this.clientName = clientName;
    }
}



public class PositionUpdateMessage: Message
{
    public Vector3 position;
    public Quaternion rotation;

    public PositionUpdateMessage(int clientID, Vector3 position, Quaternion rotation)
    {
        this.clientID = clientID;
        this.message = (int)messageTypes.PositionUpdate;
        this.position = position;
        this.rotation = rotation;
    }
}



public class RaycastHitMessage: Message
{
    public int targetID;

    public RaycastHitMessage(int clientID, int targetID)
    {
        this.clientID = clientID;
        this.message = (int)messageTypes.RaycastMessage;
        this.targetID = targetID;
    }
}
