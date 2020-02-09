using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkInstantiate : MonoBehaviour
{

    public GameObject[] prefabs;
    private Client client;
    public enum prefabNames
    {
        fireworks,
        bullet,
        ragdoll
    }

    public void setClient(Client client)
    {
        this.client = client;
    }

    public void instantiate(prefabNames name, Vector3 position, Quaternion rotation, int id)
    {
        if(client != null)
        {
            InstantiateObject instantiateObject = new InstantiateObject(id, name, position, rotation);
            client.sendToServer(instantiateObject.constructMessage());
        }
        else
        {
            Debug.LogError("Could not find client");
        }
    }

    public void recieveInstance(string message)
    {
        InstantiateObject instantiateObject = JsonUtility.FromJson<InstantiateObject>(message);
        GameObject newObject = prefabs[(int)instantiateObject.prefabName];

        Instantiate(newObject, instantiateObject.position, instantiateObject.rotation);
    }
}
