﻿using System.Collections;
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
        RaycastMessage,
        InstantiateObject,
        SwitchWeapon
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
    public bool isInProgress;

    public ConnectMessage(int clientID, int newID, bool isInProgress)
    {
        this.clientID = clientID;
        this.message = (int)messageTypes.ConnectionSuccessful;
        this.newLocalID = newID;
        this.isInProgress = isInProgress;
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
    public int raycastValue;

    public RaycastHitMessage(int clientID, int targetID)
    {
        this.clientID = clientID;
        this.message = (int)messageTypes.RaycastMessage;
        this.targetID = targetID;
    }

    public RaycastHitMessage(int clientID, int targetID, int raycastValue)
    {
        this.clientID = clientID;
        this.message = (int)messageTypes.RaycastMessage;
        this.targetID = targetID;
        this.raycastValue = raycastValue;
    }
}



public class InstantiateObject: Message
{
    public NetworkInstantiate.prefabNames prefabName;
    public Vector3 position;
    public Quaternion rotation;

    public InstantiateObject(int clientID, NetworkInstantiate.prefabNames prefabName, Vector3 position, Quaternion rotation)
    {
        this.clientID = clientID;
        this.message = (int)messageTypes.InstantiateObject;
        this.prefabName = prefabName;
        this.position = position;
        this.rotation = rotation;
    }
}


public class SwitchWeapon: Message
{
    public int weaponID;

    public SwitchWeapon(int clientID, int weaponID)
    {
        this.clientID = clientID;
        this.message = (int)messageTypes.SwitchWeapon;
        this.weaponID = weaponID;
    }
}
