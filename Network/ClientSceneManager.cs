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
    [SerializeField] private float clientRotateSmoothness = 0.5f;

    public Transform playerTransform;
    public Transform rotationTransform;

    public GameObject playerPrefab;
    public List<GameObject> remoteGameObjects;


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

            remoteGameObjects.Add(Instantiate(playerPrefab, new Vector3(1, 3, 1), Quaternion.identity));
        }

        if(remoteGameObjects != null)
        {
            for (int i = 0; i < remoteGameObjects.Count; i++)
            {
                // ********* TODO: This is dangerous code. The order of the two Lists remote clients and remote client objects might not be the same, updating the model of a different player than expected
                //                 While working, a more robust system that checks based on the id of the remote client should be used.
                //                 Remotes could be created with a remote controller class that contains their id and information on the scene object. 

                remoteGameObjects[i].transform.position = Vector3.Lerp(remoteGameObjects[i].transform.position, client.remoteClients[i].clientTransform, clientSmoothness);
                remoteGameObjects[i].transform.rotation = Quaternion.Lerp(remoteGameObjects[i].transform.rotation, client.remoteClients[i].clientRotation, clientRotateSmoothness);
            }
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
        client.sendPositionUpdate(playerTransform.position, rotationTransform.rotation);
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
