using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Message
{
    public int clientID;
    public int message;

    public enum messageTypes
    {
        ConnectRequest,
        DisconnectRequest,
        ConnectionSuccessful,
        NewPlayerData,
        PositionUpdate
    }

    public Message(int clientID, int message)
    {
        this.clientID = clientID;
        this.message = message;
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

    public ConnectMessage(int clientID, int message, int newID)
    {
        this.clientID = clientID;
        this.message = message;
        this.newLocalID = newID;
    }
}



public class NewPlayerMessage: Message
{
    public int newPlayerID;

    public NewPlayerMessage(int clientID, int message, int id)
    {
        this.clientID = clientID;
        this.message = message;
        this.newPlayerID = id;
    }
}



public class PositionUpdateMessage: Message
{
    public Vector3 position;
    public Quaternion rotation;

    public PositionUpdateMessage(int clientID, int message, Vector3 position, Quaternion rotation)
    {
        this.clientID = clientID;
        this.message = message;
        this.position = position;
        this.rotation = rotation;
    }
}
