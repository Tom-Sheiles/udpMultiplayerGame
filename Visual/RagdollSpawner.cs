using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] Ragdolls = null;

    NetworkInstantiate networkInstantiate = null;
    

    public void instantiateRagdoll(InstantiateObject ragdoll)
    {
        GameObject newObject = Ragdolls[ragdoll.clientID];
        Instantiate(newObject, ragdoll.position, ragdoll.rotation);
    }

}
