﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientSceneManager : MonoBehaviour
{
    Client client;
    public Transform transform;

    [SerializeField] private InputField portInput;
    [SerializeField] private InputField addressInput;

    private bool isActive = false;


    private void Start()
    {
        portInput.text = "3000";

    }

    public void Connect()
    {
        this.client = new Client();
        isActive = true;
        client.ConnectToServer(int.Parse(portInput.text), addressInput.text);
    }

    public void UpdatePosition()
    {
        client.sendPositionUpdate(transform);
        client.executeCommands();
    }
}
