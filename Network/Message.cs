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
