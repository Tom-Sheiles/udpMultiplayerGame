using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientSceneManager : MonoBehaviour
{
    Client client;

    [SerializeField] private InputField portInput;
    [SerializeField] private InputField addressInput;


    private void Start()
    {
        portInput.text = "3000";
    }

    public void Connect()
    {
        client = new Client();
        client.ConnectToServer(int.Parse(portInput.text), addressInput.text);
    }
}
