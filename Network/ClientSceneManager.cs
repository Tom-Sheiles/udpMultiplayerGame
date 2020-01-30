using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientSceneManager : MonoBehaviour
{
    Client client;
    private bool isActive = false;
    private Queue<string> mainThreadQueue;
    private RemoteController localRemoteController;
    private NetworkRaycastWeapons raycastWeapons;
    private UnityTemplateProjects.SimpleCameraController cameraController;

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

        cameraController = playerTransform.parent.gameObject.GetComponent<UnityTemplateProjects.SimpleCameraController>();

        localRemoteController = playerTransform.gameObject.GetComponent<RemoteController>();
        raycastWeapons = playerTransform.gameObject.GetComponent<NetworkRaycastWeapons>();
        localRemoteController.enabled = false;
        raycastWeapons.enabled = true;

    }

    private void Update()
    {
        if (client != null)
        {
            // Runs the main loop messages on the client object
            client.mainThread();
        }

        // Dequeues and parses main thread messages for the game client.
        if (mainThreadQueue.Count > 0)
        {
            // ********* TODO: Change this to a switch when more messages are added
            string nextMessage = mainThreadQueue.Dequeue();
            NewPlayerMessage newPlayerMessage = JsonUtility.FromJson<NewPlayerMessage>(nextMessage);

            GameObject newRemote = Instantiate(playerPrefab, new Vector3(0, -2, 0), Quaternion.identity);
            RemoteController controller = newRemote.GetComponent<RemoteController>();
            controller.initRemote(newPlayerMessage.newPlayerID);

            remoteGameObjects.Add(newRemote);
        }


        // Update the position of the remote clients
        if(remoteGameObjects != null)
        {

            for(int i = 0; i < remoteGameObjects.Count; i++)
            {
                RemoteController rem = remoteGameObjects[i].GetComponent<RemoteController>();
                if(rem.id == client.remoteClients[i].clientID)
                {
                    remoteGameObjects[i].transform.position = Vector3.Lerp(remoteGameObjects[i].transform.position, client.remoteClients[i].clientTransform, clientSmoothness);
                    remoteGameObjects[i].transform.rotation = Quaternion.Lerp(remoteGameObjects[i].transform.rotation, client.remoteClients[i].clientRotation, clientRotateSmoothness);
                }
            }

        }         
            
    }

    // Called when connect to server button is pressed, uses the values of portInput and address input.
    // Also begins sending position updates to the server. this maybe should be changed if a lobby is used.
    public void Connect()
    {
        this.client = new Client(this);
        isActive = true;
        client.ConnectToServer(int.Parse(portInput.text), addressInput.text);
        InvokeRepeating("UpdatePosition", 0, UpdateDelay);
    }

    // Sends a position and rotation update to the server after updatedelay seconds
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

    public void raycastCall(RemoteController objectHit)
    {
        client.sendRaycastHit(objectHit);
    }

    public void takeDamage()
    {
        cameraController.m_TargetCameraState.SetFromVector(new Vector3(0, -2, 0));
    }
}
