using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerRespawn : MonoBehaviour
{
    [SerializeField] GameEvent respawnEvent;
    [SerializeField] GameObject[] spawnPoints;

    public float respawnTime = 5.0f;

    public void StartRespawn()
    {
        StartCoroutine("respawnCoroutine", respawnTime);
        
    }

    private IEnumerator respawnCoroutine(float time)
    {
       
        yield return new WaitForSeconds(5);
        respawnEvent.Raise();
    }

}
