using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientSceneManager : MonoBehaviour
{
    Client client;
    private Queue<string> mainThreadQueue;
    private RemoteController localRemoteController;
    private NetworkWeaponManager raycastWeapons;
    private PlayerMovement playerMovement;
    private NetworkInstantiate networkInstantiate;
    private RagdollSpawner ragdollSpawner;

    [Header("Multiplayer UI")]
    [SerializeField] private InputField portInput;
    [SerializeField] private InputField addressInput;
    [SerializeField] private InputField nameInpupt = null;
    [SerializeField] private GameObject LobbyMenuObject;

    [Header("Update Rate Values")]
    [SerializeField] private float UpdateDelay = 0.5f;
    [SerializeField] private float clientSmoothness = 0.5f;
    [SerializeField] private float clientRotateSmoothness = 0.5f;

    public Transform playerTransform;
    public Transform rotationTransform;
    [HideInInspector] public PlayerHealth playerHealth;

    [Header("Remote Prefabs")]
    public GameObject playerPrefab;
    public GameObject nameTagPrefab;
    [HideInInspector] public List<GameObject> remoteGameObjects;

    [Header("Spawn Options")]
    public Transform[] spawnPositions;
    public bool randomSpawn;
    public GameObject lobbyCamera;


    private void Start()
    {
        portInput.text = "3000";
        mainThreadQueue = new Queue<string>();

        //localRemoteController = playerTransform.gameObject.GetComponentInChildren<RemoteController>();
        raycastWeapons =        playerTransform.gameObject.GetComponent<NetworkWeaponManager>();
        playerMovement =        playerTransform.GetComponent<PlayerMovement>();
        playerHealth =          playerTransform.GetComponent<PlayerHealth>();
        networkInstantiate =    GetComponent<NetworkInstantiate>();
        ragdollSpawner = GetComponent<RagdollSpawner>();

        //localRemoteController.enabled = false;
        playerMovement.enabled = true;

        if(raycastWeapons != null)
            raycastWeapons.enabled = true;

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1) && playerTransform.gameObject.activeInHierarchy)
        {
            //networkInstantiate.instantiate(NetworkInstantiate.prefabNames.fireworks, playerTransform.position, Quaternion.identity, client.localClientID);
            //networkInstantiate.instantiate(NetworkInstantiate.prefabNames.ragdoll, playerTransform.position, playerTransform.rotation, client.localClientID);
        }

        if (client != null)
        {
            // Runs the main loop messages on the client object
            client.mainThread();
        }

        // Dequeues and parses main thread messages for the game client.
        if (mainThreadQueue.Count > 0)
        {
            string nextMessage = mainThreadQueue.Dequeue();
            Message message = JsonUtility.FromJson<Message>(nextMessage);

            switch (message.message)
            {

                case (int)Message.messageTypes.NewPlayerData:

                NewPlayerMessage newPlayerMessage = JsonUtility.FromJson<NewPlayerMessage>(nextMessage);

                GameObject newRemote = Instantiate(playerPrefab, new Vector3(0, 2, 0), playerPrefab.transform.rotation);

                RemoteController controller = newRemote.GetComponentInChildren<RemoteController>();
                controller.initRemote(newPlayerMessage.newPlayerID);

                remoteGameObjects.Add(newRemote);

                    break;

                case (int)Message.messageTypes.SwitchWeapon:
                SwitchWeapon switchWeapon = JsonUtility.FromJson<SwitchWeapon>(nextMessage);
                if (switchWeapon.clientID == client.localClientID)
                    return;

                foreach (GameObject remote in remoteGameObjects)
                {
                    remote.GetComponentInChildren<RemoteController>().changeWeapon(switchWeapon.weaponID);
                }
                    break;
            }
        }


        // Update the position of the remote clients
        if(remoteGameObjects != null)
        {

            for(int i = 0; i < remoteGameObjects.Count; i++)
            {
                RemoteController rem = remoteGameObjects[i].GetComponentInChildren<RemoteController>();
                if(rem.id == client.remoteClients[i].clientID)
                {
                    remoteGameObjects[i].transform.localPosition = Vector3.Lerp(remoteGameObjects[i].transform.position, client.remoteClients[i].clientTransform, clientSmoothness);
                    remoteGameObjects[i].transform.localRotation = Quaternion.Lerp(remoteGameObjects[i].transform.rotation, client.remoteClients[i].clientRotation, clientRotateSmoothness);
                }
            }
        }             
    }

    // Called when connect to server button is pressed, uses the values of portInput and address input.
    // Also begins sending position updates to the server. this maybe should be changed if a lobby is used.
    public void Connect()
    {
        this.client = new Client(this);
        client.ConnectToServer(int.Parse(portInput.text), addressInput.text, nameInpupt.text);
        InvokeRepeating("UpdatePosition", 0, UpdateDelay);

        networkInstantiate.setClient(client);
    }

    public int getID()
    {
        return client.localClientID;
    }

    public Vector3 getnextspawn()
    {
        var nextSpawn = Random.Range(0, spawnPositions.Length);
        return spawnPositions[nextSpawn].transform.position;
    }

    public void spawnLocalPlayer()
    {
        int nextSpawn = Random.Range(0, spawnPositions.Length);
        playerTransform.position = spawnPositions[nextSpawn].transform.position;
        lobbyCamera.SetActive(false);
        playerTransform.gameObject.SetActive(true);
    }


    public void LobbyMenu()
    {
        if (!LobbyMenuObject.activeInHierarchy) { LobbyMenuObject.SetActive(true); }
    }

    // Sends a position and rotation update to the server after updatedelay seconds
    public void UpdatePosition()
    {
        client.sendPositionUpdate(playerTransform.position, rotationTransform.rotation);
    }


    public void instantiateRemote(string remote)
    {
        mainThreadQueue.Enqueue(remote);
    }

    public void instantiateRagdoll(InstantiateObject instantiateObject)
    {
        ragdollSpawner.instantiateRagdoll(instantiateObject);
    }


    public void raycastCall(RemoteController objectHit, int value)
    {
        client.sendRaycastHit(objectHit, value);
    }


    public void SendWeaponSwitch(int id)
    {
        SwitchWeapon switchWeapon = new SwitchWeapon(client.localClientID, id);
        client.sendToServer(switchWeapon.constructMessage());
    }

    public void sendRagdollSpawn(Vector3 deathPoint)
    {
        networkInstantiate.instantiate(NetworkInstantiate.prefabNames.ragdoll, deathPoint, playerTransform.rotation, client.localClientID);
    }


    public void RemoteWeaponSwitch(string message)
    {

        mainThreadQueue.Enqueue(message);
    }

    public void takeDamage(string message)
    {
        RaycastHitMessage raycastCommand = JsonUtility.FromJson<RaycastHitMessage>(message);
        playerHealth.takeDamage(raycastCommand.raycastValue);
    }
}
