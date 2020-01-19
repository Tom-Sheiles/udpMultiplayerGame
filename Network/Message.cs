using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message
{
    public int clientID;
    public int message;

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
    public int id;

    public NewPlayerMessage(int clientID, int message, int id)
    {
        this.clientID = clientID;
        this.message = message;
        this.id = id;
    }
}

public class PositionUpdateMessage: Message
{
    public Transform transform;

    public PositionUpdateMessage(int clientID, int message, Transform position)
    {
        this.clientID = clientID;
        this.message = message;
        this.transform = position;
    }
}
