using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientSceneManager : MonoBehaviour
{
    Client client;
    private bool isActive = false;
    private Queue<string> mainThreadQueue;

    [SerializeField] private InputField portInput;
    [SerializeField] private InputField addressInput;
    [SerializeField] private float UpdateDelay = 0.5f;
    [SerializeField] private float clientSmoothness = 0.5f;

    public Transform playerTransform;
    private Transform lastTransform;

    public GameObject playerPrefab;
    public GameObject newRemote;


    private void Start()
    {
        portInput.text = "3000";
        mainThreadQueue = new Queue<string>();

    }

    private void Update()
    {
        if (client != null)
        {
            client.mainThread();
        }

        if (mainThreadQueue.Count > 0)
        {
            // ********* TODO: Change this to a switch when more messages are added
            string nextMessage = mainThreadQueue.Dequeue();
            NewPlayerMessage newPlayerMessage = JsonUtility.FromJson<NewPlayerMessage>(nextMessage);

            newRemote = Instantiate(playerPrefab, new Vector3(1, 3, 1), Quaternion.identity);
        }

        if(newRemote != null)
        {
            newRemote.transform.position = Vector3.Lerp(newRemote.transform.position, client.remoteClients[0].clientTransform, clientSmoothness);
        }
            
            
    }

    public void Connect()
    {
        this.client = new Client(this);
        isActive = true;
        client.ConnectToServer(int.Parse(portInput.text), addressInput.text);
        InvokeRepeating("UpdatePosition", 0, UpdateDelay);
    }

    public void UpdatePosition()
    {
        client.sendPositionUpdate(playerTransform.position);
    }

    public void updateRemote()
    {
        //Vector3 vector3 = Vector3.zero;
        //newRemote.transform.position = client.remoteClients[0].clientTransform;
        //newRemote.transform.position = Vector3.SmoothDamp(newRemote.transform.position, client.remoteClients[0].clientTransform, ref vector3, clientSmoothness) ;
    }

    public void instantiateRemote(string remote)
    {
        mainThreadQueue.Enqueue(remote);
    }
}
